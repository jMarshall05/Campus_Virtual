import './App.css';
import Dashboard from './pages/users/AdminDashboard.jsx';
import ProtectedRoute from './components/auth/ProtectedRoute.jsx';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import Login from './pages/auth/Login.jsx';
import Unauthorized from './pages/auth/Unauthorized.jsx';
import Layout from './layouts/adminLayout.jsx';
import { validarToken } from './utils/auth.js';
import Profile from './pages/users/ProfileManage.jsx';
import Login2fa from './pages/auth/Login2fa.jsx';
import AddUser from './pages/users/AddUser.jsx';
import Users from './pages/users/Users.jsx';
import Groups from './pages/groups/Groups.jsx';
import Docs from './pages/docs/Docs.jsx';
import Courses from './pages/courses/Courses.jsx';
import Announcements from './pages/announcements/Announcements.jsx';
import Tasks from './pages/tasks/Tasks.jsx';
import MyTasks from './pages/tasks/MyTasks.jsx';
import Contacts from './pages/contacts/Contacts.jsx';
import Grades from './pages/grades/Grades.jsx';
import Submissions from './pages/submissions/Submissions.jsx';
import Calendar from './pages/calendar/Calendar.jsx';
import ChangePassword from './pages/users/ChangePassword.jsx';

function App() {
  const token = validarToken();
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={token ? <Navigate to="/Dashboard" replace /> : <Login />} />
        <Route path="/login" element={<Login />} />
        <Route path="login2fa" element={<Login2fa />} />
        <Route path="/unauthorized" element={<Unauthorized />} />
        <Route
          path="/addUser"
          element={
            <ProtectedRoute requiredRole="Administradores">
              <AddUser />
            </ProtectedRoute>
          }
        />

        <Route element={<Layout />}>
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Dashboard />
              </ProtectedRoute>
            }
          />
          <Route
            path="/users"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Users />
              </ProtectedRoute>
            }
          />
          <Route
            path="/courses"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Courses />
              </ProtectedRoute>
            }
          />
          <Route
            path="/groups"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Groups />
              </ProtectedRoute>
            }
          />
          <Route
            path="/docs"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Docs />
              </ProtectedRoute>
            }
          />
          <Route
            path="/announcements"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Announcements />
              </ProtectedRoute>
            }
          />
          <Route
            path="/tasks"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Tasks />
              </ProtectedRoute>
            }
          />
          <Route
            path="/my-tasks"
            element={
              <ProtectedRoute requiredRole="Estudiantes">
                <MyTasks />
              </ProtectedRoute>
            }
          />
          <Route
            path="/contacts"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Contacts />
              </ProtectedRoute>
            }
          />
          <Route
            path="/grades"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Grades />
              </ProtectedRoute>
            }
          />
          <Route
            path="/submissions"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Submissions />
              </ProtectedRoute>
            }
          />
          <Route
            path="/calendar"
            element={
              <ProtectedRoute requiredRole="Administradores">
                <Calendar />
              </ProtectedRoute>
            }
          />
          <Route path="/change-password" element={<ChangePassword />} />
          <Route path="/profile" element={<Profile />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
