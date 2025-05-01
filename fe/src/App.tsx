import { BrowserRouter, Route, Routes, useLocation } from "react-router-dom";
import "./App.css";
import Home from "./pages/User/Home/Home";
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";

function App() {
  return (
    <BrowserRouter>
      <MainRoutes />
    </BrowserRouter>
  );
}

export default App;

function MainRoutes() {
  const location = useLocation();
  const background = location.state?.background;
  return (
    <Routes location={background || location}>
      <Route path="/" element={<Home />} />
    </Routes>
  );
}
