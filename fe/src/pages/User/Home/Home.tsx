import { JSX } from "react";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import "./Home.css";

function Home(): JSX.Element {
  return (
    <div id="home">
      <Header />

      <Footer />
    </div>
  );
}

export default Home;
