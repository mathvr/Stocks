import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App/Layout/App.tsx'
import './App/Layout/styles.css'
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import {RouterProvider} from "react-router-dom";
import {router} from "./App/Router/Router.tsx";
import { Toaster } from 'react-hot-toast';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
    <Toaster />
  </React.StrictMode>
)
