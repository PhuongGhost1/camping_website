import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/auth": {
        target: "http://localhost:5184",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/auth/, "/auth"),
      },
      "/categories": {
        target: "http://localhost:5184",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/categories/, "/categories"),
      },
      "/products": {
        target: "http://localhost:5184",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/products/, "/products"),
      },
      "/users": {
        target: "http://localhost:5184",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/users/, "/users"),
      },
      "/orders": {
        target: "http://localhost:5184",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/orders/, "/orders"),
      },
      "/payments": {
        target: "http://localhost:5184",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/payments/, "/payments"),
      },
      "/uploads": {
        target: "http://localhost:5266",
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/uploads/, "/uploads"),
      },
    },
  },
});
