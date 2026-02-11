import { useState } from 'react'
import './App.css'
import Dashboard from './pages/AdminDashboard.jsx'
import ProtectedRoute from './components/ProtectedRoute';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom'
import Login from './pages/Login.jsx'
import Unauthorized from './pages/unauthorized.jsx'
import Layout from './layouts/adminLayout.jsx'
import { validarToken } from './utils/auth.js';
import Profile from './pages/ProfileManage.jsx';
import Usuarios from './pages/Usuarios.jsx';
import Login2fa from './pages/Login2fa.jsx';

function App() {
  const [count, setCount] = useState(0)
  const token = validarToken();
  return (


    <BrowserRouter>
      <Routes>

        <Route path="/" element={token ? <Navigate to="/Dashboard" replace /> : <Login />} />
        <Route path="/login" element={<Login />} />
        {/* <Route path='login2fa' element={<Login2fa/>}/> */}
        <Route path="/unauthorized" element={<Unauthorized />} />
        <Route element={<Layout />}>
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Dashboard />
              </ProtectedRoute>
            }
          />
          {<Route path="/usuarios" element={<ProtectedRoute requiredRole="Administradores">
            <Usuarios />
          </ProtectedRoute>
          } />}
          <Route path="/profile" element={<Profile />} />

        </Route>

      </Routes>
    </BrowserRouter>
  )
}

export default App
