import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import './index.css'
import { 
  createBrowserRouter,
  RouterProvider,
  Route, 
} from 'react-router-dom';
import ErrorPage from './pages/errorPage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    errorElement: <ErrorPage />
  },
  {
    path: '/attendees',
    element: <h1>Hello attendees</h1>
  }
])

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
)
