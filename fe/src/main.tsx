import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import AuthenProvider from "./hooks/AuthenContext.tsx";
import { ToastContainer } from "react-toastify";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <AuthenProvider>
      <ToastContainer position="top-right" autoClose={5000} closeOnClick />
      <App />
    </AuthenProvider>
  </StrictMode>
);
