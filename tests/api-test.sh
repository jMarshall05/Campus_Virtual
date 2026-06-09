#!/bin/bash
# =============================================================================
# Campus Virtual - Script de Pruebas API
# Todos los endpoints del backend (.NET 8)
# Base URL: http://localhost:5099
# =============================================================================
# INSTRUCCIONES:
#   1. Ejecutar el backend primero: dotnet run --project Api
#   2. Ejecutar este script: & "C:\Program Files\Git\bin\bash.exe" tests/api-test.sh
#   3. Los IDs se obtienen de las respuestas y se reutilizan en pruebas posteriores
# =============================================================================

BASE_URL="http://localhost:5099"
TOKEN=""
USER_ID=""
GROUP_ID=""
COURSE_ID=""
MATERIA_ID=""
ANUNCIO_ID=""
DOC_ID=""
TAREA_ID=""

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color
BOLD='\033[1m'

passed=0
failed=0
total=0

# ─── Helpers ──────────────────────────────────────────────────────────────────

print_section() {
    echo ""
    echo -e "${BOLD}${BLUE}═══════════════════════════════════════════════════════════════${NC}"
    echo -e "${BOLD}${BLUE}  $1${NC}"
    echo -e "${BOLD}${BLUE}═══════════════════════════════════════════════════════════════${NC}"
}

print_subsection() {
    echo ""
    echo -e "${CYAN}── $1 ──${NC}"
}

assert_status() {
    local expected=$1
    local actual=$2
    local description=$3
    total=$((total + 1))

    if [ "$actual" = "$expected" ]; then
        echo -e "  ${GREEN}✓ PASS${NC} $description (HTTP $actual)"
        passed=$((passed + 1))
    else
        echo -e "  ${RED}✗ FAIL${NC} $description (expected $expected, got $actual)"
        failed=$((failed + 1))
    fi
}

# Extrae un valor JSON simple (sin dependencias externas)
json_val() {
    local json="$1"
    local key="$2"
    echo "$json" | grep -o "\"$key\"[[:space:]]*:[[:space:]]*\"[^\"]*\"" | head -1 | sed "s/\"$key\"[[:space:]]*:[[:space:]]*\"//" | sed 's/"$//'
}

json_val_num() {
    local json="$1"
    local key="$2"
    echo "$json" | grep -o "\"$key\"[[:space:]]*:[[:space:]]*[0-9]*" | head -1 | sed "s/\"$key\"[[:space:]]*:[[:space:]]*//"
}

# ─── 1. AUTH ──────────────────────────────────────────────────────────────────

print_section "1. AUTENTICACIÓN (api/auth)"

# 1.1 Registrar usuario
print_subsection "1.1 POST /api/auth/register"
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Juan",
    "apellido": "Pérez",
    "email": "testadmin@santana.com",
    "identificacion": "301230456",
    "tipoIdentificacion": "Cedula",
    "rol": "Administradores",
    "fechaDeNacimiento": "1990-05-15",
    "password": "TestPassword123!",
    "confirmPassword": "TestPassword123!"
  }')
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
assert_status "200" "$HTTP_CODE" "Registrar usuario administrador"

# 1.2 Login
print_subsection "1.2 POST /api/auth/login"
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testadmin@santana.com",
    "password": "TestPassword123!"
  }')
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
TOKEN=$(json_val "$BODY" "token")
assert_status "200" "$HTTP_CODE" "Login exitoso"
if [ -n "$TOKEN" ]; then
    echo -e "  ${YELLOW}Token JWT obtenido: ${TOKEN:0:30}...${NC}"
else
    echo -e "  ${RED}No se pudo obtener token JWT${NC}"
fi

# 1.3 Login con credenciales incorrectas
print_subsection "1.3 POST /api/auth/login (credenciales incorrectas)"
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "testadmin@santana.com",
    "password": "wrongpassword"
  }')
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
assert_status "401" "$HTTP_CODE" "Login con credenciales incorrectas devuelve 401"

# 1.4 Habilitar 2FA
print_subsection "1.4 POST /api/auth/2fa/enable"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/2fa/enable?IdUsuario=$USER_ID")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Habilitar 2FA"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 1.5 Verificar 2FA
print_subsection "1.5 POST /api/auth/2fa/verify"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/2fa/verify?IdUsuario=$USER_ID&code=000000")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    # Código incorrecto esperado
    assert_status "400" "$HTTP_CODE" "Verificar 2FA con código inválido"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 1.6 Login con 2FA
print_subsection "1.6 POST /api/auth/2fa/login"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/2fa/login?idUsuario=$USER_ID&code=000000")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    # Código incorrecto esperado
    assert_status "400" "$HTTP_CODE" "Login 2FA con código inválido"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 1.7 Deshabilitar 2FA
print_subsection "1.7 POST /api/auth/2fa/disable"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/2fa/disable?IdUsuario=$USER_ID")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Deshabilitar 2FA"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# ─── 2. USUARIOS ──────────────────────────────────────────────────────────────

print_section "2. USUARIOS (api/users)"

# 2.1 Listar todos los usuarios
print_subsection "2.1 GET /api/users"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/users" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:200}..."
assert_status "200" "$HTTP_CODE" "Listar todos los usuarios"

# Obtener el primer usuario ID para usar en siguientes pruebas
USER_ID=$(echo "$BODY" | grep -o '"idUsuario"[[:space:]]*:[[:space:]]*"[^"]*"' | head -1 | sed 's/.*"idUsuario"[[:space:]]*:[[:space:]]*"//' | sed 's/"$//')
if [ -z "$USER_ID" ]; then
    USER_ID=$(echo "$BODY" | grep -o '"IdUsuario"[[:space:]]*:[[:space:]]*"[^"]*"' | head -1 | sed 's/.*"IdUsuario"[[:space:]]*:[[:space:]]*"//' | sed 's/"$//')
fi
echo -e "  ${YELLOW}USER_ID extraído: $USER_ID${NC}"

# 2.2 Listar usuarios por rol
print_subsection "2.2 GET /api/users/ByRol?rol=Estudiantes"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/users/ByRol?rol=Estudiantes" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:200}..."
assert_status "200" "$HTTP_CODE" "Listar usuarios por rol"

# 2.3 Obtener usuario por ID
print_subsection "2.3 GET /api/users/{IdUsuario}"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/users/$USER_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: ${BODY:0:200}..."
    assert_status "200" "$HTTP_CODE" "Obtener usuario por ID"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 2.4 Obtener usuario inexistente
print_subsection "2.4 GET /api/users/invalid-id-999"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/users/invalid-id-999" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
assert_status "404" "$HTTP_CODE" "Obtener usuario inexistente devuelve 404"

# 2.5 Editar usuario (PATCH)
print_subsection "2.5 PATCH /api/users/{IdUsuario}"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PATCH "$BASE_URL/api/users/$USER_ID" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d '{
        "nombre": "Juan Carlos",
        "apellido": "Pérez Modified",
        "telefonos": []
      }')
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Editar usuario (PATCH)"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 2.6 Editar usuario admin (PUT)
print_subsection "2.6 PUT /api/users/{IdUsuario}/admin"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$BASE_URL/api/users/$USER_ID/admin" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d '{
        "nombre": "Juan Carlos Admin",
        "apellido": "Pérez",
        "email": "testadmin@santana.com",
        "identificacion": "301230456",
        "tipoIdentificacion": "Cedula",
        "rol": "Administradores",
        "fechaDeNacimiento": "1990-05-15",
        "estado": true,
        "telefonos": []
      }')
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Editar usuario admin (PUT)"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 2.7 Exportar usuario PDF
print_subsection "2.7 GET /api/users/exportarPdf/{IdUsuario}"
if [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/user_test.pdf -X GET "$BASE_URL/api/users/exportarPdf/$USER_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    echo "  Archivo descargado: /tmp/user_test.pdf ($(stat -f%z /tmp/user_test.pdf 2>/dev/null || stat --printf='%s' /tmp/user_test.pdf 2>/dev/null || echo '?') bytes)"
    assert_status "200" "$HTTP_CODE" "Exportar usuario PDF"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay USER_ID disponible${NC}"
fi

# 2.8 Exportar todos los usuarios PDF
print_subsection "2.8 GET /api/users/exportarPdf"
RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/users_all_test.pdf -X GET "$BASE_URL/api/users/exportarPdf" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
echo "  Archivo descargado: /tmp/users_all_test.pdf"
assert_status "200" "$HTTP_CODE" "Exportar todos los usuarios PDF"

# 2.9 Exportar QR
print_subsection "2.9 GET /api/users/exportarQr"
RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/user_qr_test.png -X GET "$BASE_URL/api/users/exportarQr?IdUsuario=$USER_ID" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
echo "  Archivo descargado: /tmp/user_qr_test.png"
assert_status "200" "$HTTP_CODE" "Exportar QR de usuario"

# ─── 3. MATERIAS ──────────────────────────────────────────────────────────────

print_section "3. MATERIAS (api/materias)"

# 3.1 Crear materia
print_subsection "3.1 POST /api/materias"
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/materias" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"nombre": "Matemáticas Avanzadas"}')
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
MATERIA_ID=$(echo "$BODY" | grep -o '[0-9]*' | tail -1)
assert_status "200" "$HTTP_CODE" "Crear materia"
echo -e "  ${YELLOW}MATERIA_ID extraído: $MATERIA_ID${NC}"

# 3.2 Listar materias
print_subsection "3.2 GET /api/materias"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/materias" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Listar materias"

# 3.3 Obtener materia por nombre
print_subsection "3.3 GET /api/materias/ByName?nombre=Matemáticas"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/materias/ByName?nombre=Matem%C3%A1ticas" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Obtener materia por nombre"

# 3.4 Obtener materia por ID
print_subsection "3.4 GET /api/materias/{idMateria}"
if [ -n "$MATERIA_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/materias/$MATERIA_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Obtener materia por ID"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay MATERIA_ID disponible${NC}"
fi

# 3.5 Editar materia
print_subsection "3.5 PUT /api/materias/{IdMateria}"
if [ -n "$MATERIA_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$BASE_URL/api/materias/$MATERIA_ID" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d '{"nombre": "Matemáticas Avanzadas II"}')
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Editar materia"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay MATERIA_ID disponible${NC}"
fi

# 3.6 Cambiar estado materia
print_subsection "3.6 PATCH /api/materias/{materiaId}/estado"
if [ -n "$MATERIA_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PATCH "$BASE_URL/api/materias/$MATERIA_ID/estado" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Cambiar estado materia"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay MATERIA_ID disponible${NC}"
fi

# ─── 4. GRUPOS ────────────────────────────────────────────────────────────────

print_section "4. GRUPOS (api/groups)"

# 4.1 Crear grupo
print_subsection "4.1 POST /api/groups"
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/groups?IdUsuario=$USER_ID" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Grupo de Prueba 7mo A",
    "descripcion": "Grupo generado por script de pruebas"
  }')
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
GROUP_ID=$(echo "$BODY" | grep -o '[0-9]*' | tail -1)
assert_status "200" "$HTTP_CODE" "Crear grupo"
echo -e "  ${YELLOW}GROUP_ID extraído: $GROUP_ID${NC}"

# 4.2 Listar grupos
print_subsection "4.2 GET /api/groups"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/groups" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Listar grupos"

# 4.3 Obtener grupo por ID
print_subsection "4.3 GET /api/groups/{IdGrupo}"
if [ -n "$GROUP_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/groups/$GROUP_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Obtener grupo por ID"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay GROUP_ID disponible${NC}"
fi

# 4.4 Editar grupo
print_subsection "4.4 PUT /api/groups"
if [ -n "$GROUP_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$BASE_URL/api/groups?IdUsuario=$USER_ID" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d "{
        \"idGrupo\": $GROUP_ID,
        \"nombre\": \"Grupo Editado 7mo B\",
        \"descripcion\": \"Grupo editado por script de pruebas\",
        \"estado\": true
      }")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Editar grupo"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay GROUP_ID disponible${NC}"
fi

# 4.5 Exportar grupo PDF
print_subsection "4.5 GET /api/groups/exportarPdf/{IdGrupo}"
if [ -n "$GROUP_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/group_test.pdf -X GET "$BASE_URL/api/groups/exportarPdf/$GROUP_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    echo "  Archivo descargado: /tmp/group_test.pdf"
    assert_status "200" "$HTTP_CODE" "Exportar grupo PDF"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay GROUP_ID disponible${NC}"
fi

# 4.6 Exportar QR de grupo
print_subsection "4.6 GET /api/groups/exportarQr"
if [ -n "$GROUP_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/group_qr_test.png -X GET "$BASE_URL/api/groups/exportarQr?IdGrupo=$GROUP_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    echo "  Archivo descargado: /tmp/group_qr_test.png"
    assert_status "200" "$HTTP_CODE" "Exportar QR de grupo"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay GROUP_ID disponible${NC}"
fi

# ─── 5. CURSOS ────────────────────────────────────────────────────────────────

print_section "5. CURSOS (api/courses)"

# 5.1 Crear curso
print_subsection "5.1 POST /api/courses"
if [ -n "$MATERIA_ID" ] && [ -n "$GROUP_ID" ] && [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/courses" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d "{
        \"materiaId\": $MATERIA_ID,
        \"idProfesor\": \"$USER_ID\",
        \"grupoId\": $GROUP_ID,
        \"estado\": true
      }")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    COURSE_ID=$(echo "$BODY" | grep -o '[0-9]*' | tail -1)
    assert_status "200" "$HTTP_CODE" "Crear curso"
    echo -e "  ${YELLOW}COURSE_ID extraído: $COURSE_ID${NC}"
else
    echo -e "  ${YELLOW}⚠ SKIP - Se necesita MATERIA_ID, GROUP_ID y USER_ID${NC}"
fi

# 5.2 Listar cursos
print_subsection "5.2 GET /api/courses"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/courses" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Listar cursos"

# 5.3 Obtener curso por ID
print_subsection "5.3 GET /api/courses/{idCurso}"
if [ -n "$COURSE_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/courses/$COURSE_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Obtener curso por ID"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay COURSE_ID disponible${NC}"
fi

# 5.4 Cambiar estado curso
print_subsection "5.4 PATCH /api/courses/{idCurso}"
if [ -n "$COURSE_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PATCH "$BASE_URL/api/courses/$COURSE_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Cambiar estado curso"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay COURSE_ID disponible${NC}"
fi

# 5.5 Exportar cursos PDF
print_subsection "5.5 GET /api/courses/exportarPdf"
RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/courses_test.pdf -X GET "$BASE_URL/api/courses/exportarPdf" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
echo "  Archivo descargado: /tmp/courses_test.pdf"
assert_status "200" "$HTTP_CODE" "Exportar cursos PDF"

# ─── 6. TAREAS ────────────────────────────────────────────────────────────────

print_section "6. TAREAS (api/tareas)"

# 6.1 Crear tarea
print_subsection "6.1 POST /api/tareas"
if [ -n "$GROUP_ID" ] && [ -n "$MATERIA_ID" ] && [ -n "$USER_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/tareas" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d "{
        \"titulo\": \"Tarea de Prueba - Álgebra Lineal\",
        \"descripcion\": \"Ejercicios del capítulo 3\",
        \"fechaEntrega\": \"2026-06-15T23:59:00\",
        \"idGrupo\": $GROUP_ID,
        \"idMateria\": $MATERIA_ID,
        \"asignado_por\": \"$USER_ID\"
      }")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    TAREA_ID=$(echo "$BODY" | grep -o '[0-9]*' | tail -1)
    assert_status "200" "$HTTP_CODE" "Crear tarea"
    echo -e "  ${YELLOW}TAREA_ID extraído: $TAREA_ID${NC}"
else
    echo -e "  ${YELLOW}⚠ SKIP - Se necesita GROUP_ID, MATERIA_ID y USER_ID${NC}"
fi

# 6.2 Listar tareas
print_subsection "6.2 GET /api/tareas"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/tareas" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Listar tareas"

# 6.3 Obtener tarea por ID
print_subsection "6.3 GET /api/tareas/{idTarea}"
if [ -n "$TAREA_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/tareas/$TAREA_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Obtener tarea por ID"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay TAREA_ID disponible${NC}"
fi

# 6.4 Obtener tarea inexistente
print_subsection "6.4 GET /api/tareas/99999"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/tareas/99999" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
assert_status "404" "$HTTP_CODE" "Obtener tarea inexistente devuelve 404"

# 6.5 Listar tareas por grupo
print_subsection "6.5 GET /api/tareas/grupo/{IdGrupo}"
if [ -n "$GROUP_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/tareas/grupo/$GROUP_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: ${BODY:0:300}..."
    assert_status "200" "$HTTP_CODE" "Listar tareas por grupo"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay GROUP_ID disponible${NC}"
fi

# 6.6 Editar tarea
print_subsection "6.6 PUT /api/tareas/{idTarea}"
if [ -n "$TAREA_ID" ] && [ -n "$GROUP_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$BASE_URL/api/tareas/$TAREA_ID" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -d "{
        \"titulo\": \"Tarea Editada - Geometría\",
        \"descripcion\": \"Ejercicios actualizados\",
        \"fechaEntrega\": \"2026-06-20T23:59:00\",
        \"idGrupo\": $GROUP_ID
      }")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "202" "$HTTP_CODE" "Editar tarea"
else
    echo -e "  ${YELLOW}⚠ SKIP - Se necesita TAREA_ID y GROUP_ID${NC}"
fi

# 6.7 Cambiar estado tarea
print_subsection "6.7 PATCH /api/tareas/{idTarea}/estado"
if [ -n "$TAREA_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PATCH "$BASE_URL/api/tareas/$TAREA_ID/estado" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Cambiar estado tarea"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay TAREA_ID disponible${NC}"
fi

# ─── 7. ANUNCIOS ──────────────────────────────────────────────────────────────

print_section "7. ANUNCIOS (api/announcements)"

# 7.1 Crear anuncio (sin imagen)
print_subsection "7.1 POST /api/announcements (sin imagen)"
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/announcements" \
  -H "Authorization: Bearer $TOKEN" \
  -F "Titulo=Anuncio de Prueba" \
  -F "Descripcion=Este es un anuncio generado por el script de pruebas" \
  -F "FechaEvento=2026-06-15T10:00:00" \
  -F "FechaPublicacion=2026-06-09T08:00:00" \
  -F "Estado=true")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
ANUNCIO_ID=$(echo "$BODY" | grep -o '[0-9]*' | tail -1)
assert_status "200" "$HTTP_CODE" "Crear anuncio sin imagen"
echo -e "  ${YELLOW}ANUNCIO_ID extraído: $ANUNCIO_ID${NC}"

# 7.2 Crear anuncio con imagen
print_subsection "7.2 POST /api/announcements (con imagen)"
# Crear una imagen dummy para probar
echo -e "\x89PNG\r\n\x1a\n" > /tmp/test_image.png
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/announcements" \
  -H "Authorization: Bearer $TOKEN" \
  -F "Titulo=Anuncio con Imagen" \
  -F "Descripcion=Anuncio con imagen de prueba" \
  -F "FechaEvento=2026-06-20T10:00:00" \
  -F "FechaPublicacion=2026-06-09T08:00:00" \
  -F "Estado=true" \
  -F "Imagen=@/tmp/test_image.png")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
assert_status "200" "$HTTP_CODE" "Crear anuncio con imagen"

# 7.3 Listar anuncios
print_subsection "7.3 GET /api/announcements"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/announcements" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Listar anuncios"

# 7.4 Obtener anuncio por ID
print_subsection "7.4 GET /api/announcements/{id}"
if [ -n "$ANUNCIO_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/announcements/$ANUNCIO_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Obtener anuncio por ID"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay ANUNCIO_ID disponible${NC}"
fi

# 7.5 Editar anuncio
print_subsection "7.5 PUT /api/announcements"
if [ -n "$ANUNCIO_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$BASE_URL/api/announcements" \
      -H "Authorization: Bearer $TOKEN" \
      -F "IdAnuncio=$ANUNCIO_ID" \
      -F "Titulo=Anuncio Editado" \
      -F "Descripcion=Anuncio actualizado por script" \
      -F "FechaEvento=2026-06-25T10:00:00" \
      -F "FechaPublicacion=2026-06-09T09:00:00" \
      -F "Estado=true")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Editar anuncio"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay ANUNCIO_ID disponible${NC}"
fi

# 7.6 Cambiar estado anuncio
print_subsection "7.6 PATCH /api/announcements/{anuncioId}"
if [ -n "$ANUNCIO_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PATCH "$BASE_URL/api/announcements/$ANUNCIO_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Cambiar estado anuncio"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay ANUNCIO_ID disponible${NC}"
fi

# 7.7 Obtener imagen del anuncio
print_subsection "7.7 GET /api/announcements/{id}/image"
if [ -n "$ANUNCIO_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/anuncio_img_test -X GET "$BASE_URL/api/announcements/$ANUNCIO_ID/image" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    echo "  Imagen descargada: /tmp/anuncio_img_test"
    # Puede ser 204 si no tiene imagen
    if [ "$HTTP_CODE" = "200" ] || [ "$HTTP_CODE" = "204" ]; then
        assert_status "$HTTP_CODE" "$HTTP_CODE" "Obtener imagen anuncio (HTTP $HTTP_CODE)"
    else
        assert_status "200" "$HTTP_CODE" "Obtener imagen anuncio"
    fi
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay ANUNCIO_ID disponible${NC}"
fi

# ─── 8. DOCUMENTOS ────────────────────────────────────────────────────────────

print_section "8. DOCUMENTOS (api/docs)"

# 8.1 Crear documento
print_subsection "8.1 POST /api/docs"
echo "Documento de prueba contenido" > /tmp/test_doc.txt
RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/docs" \
  -H "Authorization: Bearer $TOKEN" \
  -F "Titulo=Documento de Prueba" \
  -F "Descripcion=Documento generado por script" \
  -F "Categoria=Académico" \
  -F "Doc=@/tmp/test_doc.txt")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: $BODY"
DOC_ID=$(echo "$BODY" | grep -o '"id":[0-9]*' | head -1 | sed 's/"id"://')
if [ -z "$DOC_ID" ]; then
    DOC_ID=$(echo "$BODY" | grep -o '"Id":[0-9]*' | head -1 | sed 's/"Id"://')
fi
assert_status "200" "$HTTP_CODE" "Crear documento"
echo -e "  ${YELLOW}DOC_ID extraído: $DOC_ID${NC}"

# 8.2 Listar documentos
print_subsection "8.2 GET /api/docs"
RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$BASE_URL/api/docs" \
  -H "Authorization: Bearer $TOKEN")
HTTP_CODE=$(echo "$RESPONSE" | tail -1)
BODY=$(echo "$RESPONSE" | sed '$d')
echo "  Response: ${BODY:0:300}..."
assert_status "200" "$HTTP_CODE" "Listar documentos"

# 8.3 Descargar documento
print_subsection "8.3 GET /api/docs/download/{Id}"
if [ -n "$DOC_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -o /tmp/doc_downloaded.txt -X GET "$BASE_URL/api/docs/download/$DOC_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    echo "  Archivo descargado: /tmp/doc_downloaded.txt"
    assert_status "200" "$HTTP_CODE" "Descargar documento"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay DOC_ID disponible${NC}"
fi

# 8.4 Editar documento
print_subsection "8.4 PUT /api/docs/{idDocumento}"
if [ -n "$DOC_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$BASE_URL/api/docs/$DOC_ID" \
      -H "Authorization: Bearer $TOKEN" \
      -F "Titulo=Documento Editado" \
      -F "Descripcion=Documento actualizado" \
      -F "Categoria=Administrativo")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "200" "$HTTP_CODE" "Editar documento"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay DOC_ID disponible${NC}"
fi

# 8.5 Borrar documento
print_subsection "8.5 DELETE /api/docs/{idDocumento}"
if [ -n "$DOC_ID" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X DELETE "$BASE_URL/api/docs/$DOC_ID" \
      -H "Authorization: Bearer $TOKEN")
    HTTP_CODE=$(echo "$RESPONSE" | tail -1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    echo "  Response: $BODY"
    assert_status "204" "$HTTP_CODE" "Borrar documento"
else
    echo -e "  ${YELLOW}⚠ SKIP - No hay DOC_ID disponible${NC}"
fi

# ─── RESUMEN ──────────────────────────────────────────────────────────────────

print_section "RESUMEN DE PRUEBAS"
echo ""
echo -e "${BOLD}  Total:  $total${NC}"
echo -e "${GREEN}  Passed: $passed${NC}"
echo -e "${RED}  Failed: $failed${NC}"
echo ""

if [ "$failed" -eq 0 ]; then
    echo -e "${GREEN}${BOLD}  🎉 ¡TODAS LAS PRUEBAS PASARON!${NC}"
else
    echo -e "${RED}${BOLD}  ⚠  $failed prueba(s) fallaron${NC}"
fi

echo ""
echo -e "${BOLD}  IDs generados para uso posterior:${NC}"
echo -e "  USER_ID:    $USER_ID"
echo -e "  GROUP_ID:   $GROUP_ID"
echo -e "  MATERIA_ID: $MATERIA_ID"
echo -e "  COURSE_ID:  $COURSE_ID"
echo -e "  TAREA_ID:   $TAREA_ID"
echo -e "  ANUNCIO_ID: $ANUNCIO_ID"
echo -e "  DOC_ID:     $DOC_ID"
echo ""
