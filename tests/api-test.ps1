# =============================================================================
# Campus Virtual - Script de Pruebas API (PowerShell)
# Todos los endpoints del backend (.NET 8)
# Base URL: http://localhost:5099
# =============================================================================
# INSTRUCCIONES:
#   1. Ejecutar el backend primero: dotnet run --project Api
#   2. Ejecutar este script: .\tests\api-test.ps1
#   3. Los IDs se obtienen de las respuestas y se reutilizan en pruebas posteriores
# =============================================================================

$BASE_URL = "http://localhost:5099"
$headers = @{}
$passed = 0
$failed = 0
$total = 0

# Variables globales de IDs
$script:TOKEN = ""
$script:USER_ID = ""
$script:GROUP_ID = ""
$script:COURSE_ID = ""
$script:MATERIA_ID = ""
$script:ANUNCIO_ID = ""
$script:DOC_ID = ""
$script:TAREA_ID = ""

# ─── Helpers ──────────────────────────────────────────────────────────────────

function Print-Section($text) {
    Write-Host ""
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Blue
    Write-Host "  $text" -ForegroundColor Blue
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Blue
}

function Print-Subsection($text) {
    Write-Host ""
    Write-Host "── $text ──" -ForegroundColor Cyan
}

function Assert-Status($expected, $actual, $description) {
    $script:total++
    if ("$actual" -eq "$expected") {
        Write-Host "  ✓ PASS $description (HTTP $actual)" -ForegroundColor Green
        $script:passed++
    } else {
        Write-Host "  ✗ FAIL $description (expected $expected, got $actual)" -ForegroundColor Red
        $script:failed++
    }
}

function Invoke-Test {
    param(
        [string]$Method,
        [string]$Url,
        [hashtable]$Body = $null,
        [hashtable]$Form = $null,
        [string]$ContentType = "application/json",
        [int]$ExpectedStatus = 200
    )

    try {
        $params = @{
            Uri = $Url
            Method = $Method
            Headers = $headers
        }

        if ($Body) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10)
            $params.ContentType = $ContentType
        }

        if ($Form) {
            # Para multipart, usamos Invoke-WebRequest
            $response = Invoke-WebRequest @params -Form $Form -ErrorAction Stop
            return @{
                StatusCode = [int]$response.StatusCode
                Content = $response.Content
            }
        }

        $response = Invoke-WebRequest @params -ErrorAction Stop
        return @{
            StatusCode = [int]$response.StatusCode
            Content = $response.Content
        }
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        $content = ""
        try {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $content = $reader.ReadToEnd()
        } catch {}
        return @{
            StatusCode = $statusCode
            Content = $content
        }
    }
}

# ═══════════════════════════════════════════════════════════════════════════════
# 1. AUTENTICACIÓN
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "1. AUTENTICACIÓN (api/auth)"

# 1.1 Registrar usuario
Print-Subsection "1.1 POST /api/auth/register"
$registerBody = @{
    nombre = "Juan"
    apellido = "Pérez"
    email = "testadmin@santana.com"
    identificacion = "301230456"
    tipoIdentificacion = "Cedula"
    rol = "Administradores"
    fechaDeNacimiento = "1990-05-15"
    password = "TestPassword123!"
    confirmPassword = "TestPassword123!"
}
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/register" -Body $registerBody
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(200, $result.Content.Length)))"
Assert-Status 200 $result.StatusCode "Registrar usuario administrador"

# 1.2 Login
Print-Subsection "1.2 POST /api/auth/login"
$loginBody = @{
    email = "testadmin@santana.com"
    password = "TestPassword123!"
}
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/login" -Body $loginBody
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(200, $result.Content.Length)))"
$json = $result.Content | ConvertFrom-Json
if ($json.token) {
    $script:TOKEN = $json.token
    $headers["Authorization"] = "Bearer $($script:TOKEN)"
    Write-Host "  Token JWT obtenido: $($script:TOKEN.Substring(0, [Math]::Min(30, $script:TOKEN.Length)))..." -ForegroundColor Yellow
}
Assert-Status 200 $result.StatusCode "Login exitoso"

# 1.3 Login con credenciales incorrectas
Print-Subsection "1.3 POST /api/auth/login (credenciales incorrectas)"
$badLoginBody = @{
    email = "testadmin@santana.com"
    password = "wrongpassword"
}
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/login" -Body $badLoginBody -ExpectedStatus 401
Write-Host "  Response: $($result.Content)"
Assert-Status 401 $result.StatusCode "Login con credenciales incorrectas devuelve 401"

# 1.4 Habilitar 2FA
Print-Subsection "1.4 POST /api/auth/2fa/enable"
if ($script:USER_ID) {
    $result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/2fa/enable?IdUsuario=$($script:USER_ID)"
    Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(200, $result.Content.Length)))"
    Assert-Status 200 $result.StatusCode "Habilitar 2FA"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 1.5 Verificar 2FA (código inválido)
Print-Subsection "1.5 POST /api/auth/2fa/verify"
if ($script:USER_ID) {
    $result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/2fa/verify?IdUsuario=$($script:USER_ID)&code=000000" -ExpectedStatus 400
    Write-Host "  Response: $($result.Content)"
    Assert-Status 400 $result.StatusCode "Verificar 2FA con código inválido"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 1.6 Login 2FA (código inválido)
Print-Subsection "1.6 POST /api/auth/2fa/login"
if ($script:USER_ID) {
    $result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/2fa/login?idUsuario=$($script:USER_ID)&code=000000" -ExpectedStatus 400
    Write-Host "  Response: $($result.Content)"
    Assert-Status 400 $result.StatusCode "Login 2FA con código inválido"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 1.7 Deshabilitar 2FA
Print-Subsection "1.7 POST /api/auth/2fa/disable"
if ($script:USER_ID) {
    $result = Invoke-Test -Method POST -Url "$BASE_URL/api/auth/2fa/disable?IdUsuario=$($script:USER_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Deshabilitar 2FA"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# 2. USUARIOS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "2. USUARIOS (api/users)"

# 2.1 Listar todos los usuarios
Print-Subsection "2.1 GET /api/users"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/users"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(200, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar todos los usuarios"
# Extraer primer usuario
$users = $result.Content | ConvertFrom-Json
if ($users -and $users.Count -gt 0) {
    $script:USER_ID = $users[0].idUsuario
    if (-not $script:USER_ID) { $script:USER_ID = $users[0].IdUsuario }
    Write-Host "  USER_ID extraído: $($script:USER_ID)" -ForegroundColor Yellow
}

# 2.2 Listar por rol
Print-Subsection "2.2 GET /api/users/ByRol?rol=Estudiantes"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/users/ByRol?rol=Estudiantes"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(200, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar usuarios por rol"

# 2.3 Obtener usuario por ID
Print-Subsection "2.3 GET /api/users/{IdUsuario}"
if ($script:USER_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/users/$($script:USER_ID)"
    Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(200, $result.Content.Length)))..."
    Assert-Status 200 $result.StatusCode "Obtener usuario por ID"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 2.4 Usuario inexistente
Print-Subsection "2.4 GET /api/users/invalid-id-999"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/users/invalid-id-999" -ExpectedStatus 404
Write-Host "  Response: $($result.Content)"
Assert-Status 404 $result.StatusCode "Obtener usuario inexistente devuelve 404"

# 2.5 Editar usuario (PATCH)
Print-Subsection "2.5 PATCH /api/users/{IdUsuario}"
if ($script:USER_ID) {
    $editBody = @{
        nombre = "Juan Carlos"
        apellido = "Pérez Modified"
        telefonos = @()
    }
    $result = Invoke-Test -Method PATCH -Url "$BASE_URL/api/users/$($script:USER_ID)" -Body $editBody
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Editar usuario (PATCH)"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 2.6 Editar usuario admin (PUT)
Print-Subsection "2.6 PUT /api/users/{IdUsuario}/admin"
if ($script:USER_ID) {
    $adminBody = @{
        nombre = "Juan Carlos Admin"
        apellido = "Pérez"
        email = "testadmin@santana.com"
        identificacion = "301230456"
        tipoIdentificacion = "Cedula"
        rol = "Administradores"
        fechaDeNacimiento = "1990-05-15"
        estado = $true
        telefonos = @()
    }
    $result = Invoke-Test -Method PUT -Url "$BASE_URL/api/users/$($script:USER_ID)/admin" -Body $adminBody
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Editar usuario admin (PUT)"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 2.7 Exportar usuario PDF
Print-Subsection "2.7 GET /api/users/exportarPdf/{IdUsuario}"
if ($script:USER_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/users/exportarPdf/$($script:USER_ID)"
    Assert-Status 200 $result.StatusCode "Exportar usuario PDF"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# 2.8 Exportar todos PDF
Print-Subsection "2.8 GET /api/users/exportarPdf"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/users/exportarPdf"
Assert-Status 200 $result.StatusCode "Exportar todos los usuarios PDF"

# 2.9 Exportar QR
Print-Subsection "2.9 GET /api/users/exportarQr"
if ($script:USER_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/users/exportarQr?IdUsuario=$($script:USER_ID)"
    Assert-Status 200 $result.StatusCode "Exportar QR de usuario"
} else {
    Write-Host "  ⚠ SKIP - No hay USER_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# 3. MATERIAS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "3. MATERIAS (api/materias)"

# 3.1 Crear materia
Print-Subsection "3.1 POST /api/materias"
$materiaBody = @{ nombre = "Matemáticas Avanzadas" }
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/materias" -Body $materiaBody
Write-Host "  Response: $($result.Content)"
Assert-Status 200 $result.StatusCode "Crear materia"
$script:MATERIA_ID = ($result.Content -replace '[^0-9]', '') | ForEach-Object { if ($_ -match '\d+') { $matches[0] } }
Write-Host "  MATERIA_ID extraído: $($script:MATERIA_ID)" -ForegroundColor Yellow

# 3.2 Listar materias
Print-Subsection "3.2 GET /api/materias"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/materias"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar materias"

# 3.3 Buscar por nombre
Print-Subsection "3.3 GET /api/materias/ByName?nombre=Matemáticas"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/materias/ByName?nombre=Matematicas"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Obtener materia por nombre"

# 3.4 Obtener por ID
Print-Subsection "3.4 GET /api/materias/{idMateria}"
if ($script:MATERIA_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/materias/$($script:MATERIA_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Obtener materia por ID"
} else {
    Write-Host "  ⚠ SKIP - No hay MATERIA_ID disponible" -ForegroundColor Yellow
}

# 3.5 Editar materia
Print-Subsection "3.5 PUT /api/materias/{IdMateria}"
if ($script:MATERIA_ID) {
    $editMatBody = @{ nombre = "Matemáticas Avanzadas II" }
    $result = Invoke-Test -Method PUT -Url "$BASE_URL/api/materias/$($script:MATERIA_ID)" -Body $editMatBody
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Editar materia"
} else {
    Write-Host "  ⚠ SKIP - No hay MATERIA_ID disponible" -ForegroundColor Yellow
}

# 3.6 Cambiar estado
Print-Subsection "3.6 PATCH /api/materias/{materiaId}/estado"
if ($script:MATERIA_ID) {
    $result = Invoke-Test -Method PATCH -Url "$BASE_URL/api/materias/$($script:MATERIA_ID)/estado"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Cambiar estado materia"
} else {
    Write-Host "  ⚠ SKIP - No hay MATERIA_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# 4. GRUPOS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "4. GRUPOS (api/groups)"

# 4.1 Crear grupo
Print-Subsection "4.1 POST /api/groups"
$grupoBody = @{
    nombre = "Grupo de Prueba 7mo A"
    descripcion = "Grupo generado por script de pruebas"
}
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/groups?IdUsuario=$($script:USER_ID)" -Body $grupoBody
Write-Host "  Response: $($result.Content)"
Assert-Status 200 $result.StatusCode "Crear grupo"
$script:GROUP_ID = ($result.Content -replace '[^0-9]', '') | ForEach-Object { if ($_ -match '\d+') { $matches[0] } }
Write-Host "  GROUP_ID extraído: $($script:GROUP_ID)" -ForegroundColor Yellow

# 4.2 Listar grupos
Print-Subsection "4.2 GET /api/groups"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/groups"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar grupos"

# 4.3 Obtener grupo por ID
Print-Subsection "4.3 GET /api/groups/{IdGrupo}"
if ($script:GROUP_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/groups/$($script:GROUP_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Obtener grupo por ID"
} else {
    Write-Host "  ⚠ SKIP - No hay GROUP_ID disponible" -ForegroundColor Yellow
}

# 4.4 Editar grupo
Print-Subsection "4.4 PUT /api/groups"
if ($script:GROUP_ID) {
    $editGrupoBody = @{
        idGrupo = [int]$script:GROUP_ID
        nombre = "Grupo Editado 7mo B"
        descripcion = "Grupo editado por script"
        estado = $true
    }
    $result = Invoke-Test -Method PUT -Url "$BASE_URL/api/groups?IdUsuario=$($script:USER_ID)" -Body $editGrupoBody
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Editar grupo"
} else {
    Write-Host "  ⚠ SKIP - No hay GROUP_ID disponible" -ForegroundColor Yellow
}

# 4.5 Exportar PDF
Print-Subsection "4.5 GET /api/groups/exportarPdf/{IdGrupo}"
if ($script:GROUP_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/groups/exportarPdf/$($script:GROUP_ID)"
    Assert-Status 200 $result.StatusCode "Exportar grupo PDF"
} else {
    Write-Host "  ⚠ SKIP - No hay GROUP_ID disponible" -ForegroundColor Yellow
}

# 4.6 Exportar QR
Print-Subsection "4.6 GET /api/groups/exportarQr"
if ($script:GROUP_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/groups/exportarQr?IdGrupo=$($script:GROUP_ID)"
    Assert-Status 200 $result.StatusCode "Exportar QR de grupo"
} else {
    Write-Host "  ⚠ SKIP - No hay GROUP_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# 5. CURSOS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "5. CURSOS (api/courses)"

# 5.1 Crear curso
Print-Subsection "5.1 POST /api/courses"
if ($script:MATERIA_ID -and $script:GROUP_ID -and $script:USER_ID) {
    $cursoBody = @{
        materiaId = [int]$script:MATERIA_ID
        idProfesor = $script:USER_ID
        grupoId = [int]$script:GROUP_ID
        estado = $true
    }
    $result = Invoke-Test -Method POST -Url "$BASE_URL/api/courses" -Body $cursoBody
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Crear curso"
    $script:COURSE_ID = ($result.Content -replace '[^0-9]', '') | ForEach-Object { if ($_ -match '\d+') { $matches[0] } }
    Write-Host "  COURSE_ID extraído: $($script:COURSE_ID)" -ForegroundColor Yellow
} else {
    Write-Host "  ⚠ SKIP - Se necesita MATERIA_ID, GROUP_ID y USER_ID" -ForegroundColor Yellow
}

# 5.2 Listar cursos
Print-Subsection "5.2 GET /api/courses"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/courses"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar cursos"

# 5.3 Obtener curso por ID
Print-Subsection "5.3 GET /api/courses/{idCurso}"
if ($script:COURSE_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/courses/$($script:COURSE_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Obtener curso por ID"
} else {
    Write-Host "  ⚠ SKIP - No hay COURSE_ID disponible" -ForegroundColor Yellow
}

# 5.4 Cambiar estado
Print-Subsection "5.4 PATCH /api/courses/{idCurso}"
if ($script:COURSE_ID) {
    $result = Invoke-Test -Method PATCH -Url "$BASE_URL/api/courses/$($script:COURSE_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Cambiar estado curso"
} else {
    Write-Host "  ⚠ SKIP - No hay COURSE_ID disponible" -ForegroundColor Yellow
}

# 5.5 Exportar PDF
Print-Subsection "5.5 GET /api/courses/exportarPdf"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/courses/exportarPdf"
Assert-Status 200 $result.StatusCode "Exportar cursos PDF"

# ═══════════════════════════════════════════════════════════════════════════════
# 6. TAREAS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "6. TAREAS (api/tareas)"

# 6.1 Crear tarea
Print-Subsection "6.1 POST /api/tareas"
if ($script:GROUP_ID -and $script:MATERIA_ID -and $script:USER_ID) {
    $tareaBody = @{
        titulo = "Tarea de Prueba - Álgebra Lineal"
        descripcion = "Ejercicios del capítulo 3"
        fechaEntrega = "2026-06-15T23:59:00"
        idGrupo = [int]$script:GROUP_ID
        idMateria = [int]$script:MATERIA_ID
        asignado_por = $script:USER_ID
    }
    $result = Invoke-Test -Method POST -Url "$BASE_URL/api/tareas" -Body $tareaBody
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Crear tarea"
    $script:TAREA_ID = ($result.Content -replace '[^0-9]', '') | ForEach-Object { if ($_ -match '\d+') { $matches[0] } }
    Write-Host "  TAREA_ID extraído: $($script:TAREA_ID)" -ForegroundColor Yellow
} else {
    Write-Host "  ⚠ SKIP - Se necesita GROUP_ID, MATERIA_ID y USER_ID" -ForegroundColor Yellow
}

# 6.2 Listar tareas
Print-Subsection "6.2 GET /api/tareas"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/tareas"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar tareas"

# 6.3 Obtener tarea por ID
Print-Subsection "6.3 GET /api/tareas/{idTarea}"
if ($script:TAREA_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/tareas/$($script:TAREA_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Obtener tarea por ID"
} else {
    Write-Host "  ⚠ SKIP - No hay TAREA_ID disponible" -ForegroundColor Yellow
}

# 6.4 Tarea inexistente
Print-Subsection "6.4 GET /api/tareas/99999"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/tareas/99999" -ExpectedStatus 404
Write-Host "  Response: $($result.Content)"
Assert-Status 404 $result.StatusCode "Obtener tarea inexistente devuelve 404"

# 6.5 Tareas por grupo
Print-Subsection "6.5 GET /api/tareas/grupo/{IdGrupo}"
if ($script:GROUP_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/tareas/grupo/$($script:GROUP_ID)"
    Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
    Assert-Status 200 $result.StatusCode "Listar tareas por grupo"
} else {
    Write-Host "  ⚠ SKIP - No hay GROUP_ID disponible" -ForegroundColor Yellow
}

# 6.6 Editar tarea
Print-Subsection "6.6 PUT /api/tareas/{idTarea}"
if ($script:TAREA_ID -and $script:GROUP_ID) {
    $editTareaBody = @{
        titulo = "Tarea Editada - Geometría"
        descripcion = "Ejercicios actualizados"
        fechaEntrega = "2026-06-20T23:59:00"
        idGrupo = [int]$script:GROUP_ID
    }
    $result = Invoke-Test -Method PUT -Url "$BASE_URL/api/tareas/$($script:TAREA_ID)" -Body $editTareaBody -ExpectedStatus 202
    Write-Host "  Response: $($result.Content)"
    Assert-Status 202 $result.StatusCode "Editar tarea"
} else {
    Write-Host "  ⚠ SKIP - Se necesita TAREA_ID y GROUP_ID" -ForegroundColor Yellow
}

# 6.7 Cambiar estado
Print-Subsection "6.7 PATCH /api/tareas/{idTarea}/estado"
if ($script:TAREA_ID) {
    $result = Invoke-Test -Method PATCH -Url "$BASE_URL/api/tareas/$($script:TAREA_ID)/estado"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Cambiar estado tarea"
} else {
    Write-Host "  ⚠ SKIP - No hay TAREA_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# 7. ANUNCIOS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "7. ANUNCIOS (api/announcements)"

# 7.1 Crear anuncio sin imagen
Print-Subsection "7.1 POST /api/announcements (sin imagen)"
$announceForm = @{
    Titulo = "Anuncio de Prueba"
    Descripcion = "Este es un anuncio generado por el script de pruebas"
    FechaEvento = "2026-06-15T10:00:00"
    FechaPublicacion = "2026-06-09T08:00:00"
    Estado = "true"
}
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/announcements" -Form $announceForm
Write-Host "  Response: $($result.Content)"
Assert-Status 200 $result.StatusCode "Crear anuncio sin imagen"
$script:ANUNCIO_ID = ($result.Content -replace '[^0-9]', '') | ForEach-Object { if ($_ -match '\d+') { $matches[0] } }
Write-Host "  ANUNCIO_ID extraído: $($script:ANUNCIO_ID)" -ForegroundColor Yellow

# 7.2 Listar anuncios
Print-Subsection "7.2 GET /api/announcements"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/announcements"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar anuncios"

# 7.3 Obtener por ID
Print-Subsection "7.3 GET /api/announcements/{id}"
if ($script:ANUNCIO_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/announcements/$($script:ANUNCIO_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Obtener anuncio por ID"
} else {
    Write-Host "  ⚠ SKIP - No hay ANUNCIO_ID disponible" -ForegroundColor Yellow
}

# 7.4 Editar anuncio
Print-Subsection "7.4 PUT /api/announcements"
if ($script:ANUNCIO_ID) {
    $editAnnounceForm = @{
        IdAnuncio = $script:ANUNCIO_ID
        Titulo = "Anuncio Editado"
        Descripcion = "Anuncio actualizado por script"
        FechaEvento = "2026-06-25T10:00:00"
        FechaPublicacion = "2026-06-09T09:00:00"
        Estado = "true"
    }
    $result = Invoke-Test -Method PUT -Url "$BASE_URL/api/announcements" -Form $editAnnounceForm
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Editar anuncio"
} else {
    Write-Host "  ⚠ SKIP - No hay ANUNCIO_ID disponible" -ForegroundColor Yellow
}

# 7.5 Cambiar estado
Print-Subsection "7.5 PATCH /api/announcements/{anuncioId}"
if ($script:ANUNCIO_ID) {
    $result = Invoke-Test -Method PATCH -Url "$BASE_URL/api/announcements/$($script:ANUNCIO_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Cambiar estado anuncio"
} else {
    Write-Host "  ⚠ SKIP - No hay ANUNCIO_ID disponible" -ForegroundColor Yellow
}

# 7.6 Obtener imagen
Print-Subsection "7.6 GET /api/announcements/{id}/image"
if ($script:ANUNCIO_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/announcements/$($script:ANUNCIO_ID)/image"
    if ($result.StatusCode -eq 200 -or $result.StatusCode -eq 204) {
        Assert-Status $result.StatusCode $result.StatusCode "Obtener imagen anuncio (HTTP $($result.StatusCode))"
    } else {
        Assert-Status 200 $result.StatusCode "Obtener imagen anuncio"
    }
} else {
    Write-Host "  ⚠ SKIP - No hay ANUNCIO_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# 8. DOCUMENTOS
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "8. DOCUMENTOS (api/docs)"

# 8.1 Crear documento
Print-Subsection "8.1 POST /api/docs"
$testFile = Join-Path $env:TEMP "test_doc.txt"
"Documento de prueba contenido" | Out-File -FilePath $testFile -Encoding utf8
$docForm = @{
    Titulo = "Documento de Prueba"
    Descripcion = "Documento generado por script"
    Categoria = "Académico"
    Doc = Get-Item $testFile
}
$result = Invoke-Test -Method POST -Url "$BASE_URL/api/docs" -Form $docForm
Write-Host "  Response: $($result.Content)"
Assert-Status 200 $result.StatusCode "Crear documento"
$docJson = $result.Content | ConvertFrom-Json
$script:DOC_ID = $docJson.id
Write-Host "  DOC_ID extraído: $($script:DOC_ID)" -ForegroundColor Yellow

# 8.2 Listar documentos
Print-Subsection "8.2 GET /api/docs"
$result = Invoke-Test -Method GET -Url "$BASE_URL/api/docs"
Write-Host "  Response: $($result.Content.Substring(0, [Math]::Min(300, $result.Content.Length)))..."
Assert-Status 200 $result.StatusCode "Listar documentos"

# 8.3 Descargar documento
Print-Subsection "8.3 GET /api/docs/download/{Id}"
if ($script:DOC_ID) {
    $result = Invoke-Test -Method GET -Url "$BASE_URL/api/docs/download/$($script:DOC_ID)"
    Assert-Status 200 $result.StatusCode "Descargar documento"
} else {
    Write-Host "  ⚠ SKIP - No hay DOC_ID disponible" -ForegroundColor Yellow
}

# 8.4 Editar documento
Print-Subsection "8.4 PUT /api/docs/{idDocumento}"
if ($script:DOC_ID) {
    $editDocForm = @{
        Titulo = "Documento Editado"
        Descripcion = "Documento actualizado"
        Categoria = "Administrativo"
    }
    $result = Invoke-Test -Method PUT -Url "$BASE_URL/api/docs/$($script:DOC_ID)" -Form $editDocForm
    Write-Host "  Response: $($result.Content)"
    Assert-Status 200 $result.StatusCode "Editar documento"
} else {
    Write-Host "  ⚠ SKIP - No hay DOC_ID disponible" -ForegroundColor Yellow
}

# 8.5 Borrar documento
Print-Subsection "8.5 DELETE /api/docs/{idDocumento}"
if ($script:DOC_ID) {
    $result = Invoke-Test -Method DELETE -Url "$BASE_URL/api/docs/$($script:DOC_ID)"
    Write-Host "  Response: $($result.Content)"
    Assert-Status 204 $result.StatusCode "Borrar documento"
} else {
    Write-Host "  ⚠ SKIP - No hay DOC_ID disponible" -ForegroundColor Yellow
}

# ═══════════════════════════════════════════════════════════════════════════════
# RESUMEN
# ═══════════════════════════════════════════════════════════════════════════════

Print-Section "RESUMEN DE PRUEBAS"
Write-Host ""
Write-Host "  Total:  $total"
Write-Host "  Passed: $passed" -ForegroundColor Green
Write-Host "  Failed: $failed" -ForegroundColor Red
Write-Host ""

if ($failed -eq 0) {
    Write-Host "  🎉 ¡TODAS LAS PRUEBAS PASARON!" -ForegroundColor Green
} else {
    Write-Host "  ⚠  $failed prueba(s) fallaron" -ForegroundColor Red
}

Write-Host ""
Write-Host "  IDs generados para uso posterior:"
Write-Host "  USER_ID:    $($script:USER_ID)"
Write-Host "  GROUP_ID:   $($script:GROUP_ID)"
Write-Host "  MATERIA_ID: $($script:MATERIA_ID)"
Write-Host "  COURSE_ID:  $($script:COURSE_ID)"
Write-Host "  TAREA_ID:   $($script:TAREA_ID)"
Write-Host "  ANUNCIO_ID: $($script:ANUNCIO_ID)"
Write-Host "  DOC_ID:     $($script:DOC_ID)"
Write-Host ""
