import { JSX } from "react";
import "./Hero.css";
import Category from "./Category/Category";
import img_hero_bg from "../../../../assets/hero-bg.png";
import img_arrow_right from "../../../../assets/arrow-right.png";
import img_hero from "../../../../assets/hero.png";

function Hero(): JSX.Element {
  return (
    <section className="hero" id="hero">
      <img src={img_hero_bg} alt="" className="hero-bg" />
      <div className="hero-content container">
        <h2>
          We have the <span>equipment</span> for your trip like this!
        </h2>
        <div className="hero-btn">
          <a href="#" className="btn">
            <span>View Products</span>
            <img src={img_arrow_right} alt="" />
          </a>
        </div>
        <div className="hero-img">
          <img src={img_hero} alt="" />
        </div>
      </div>

      <Category />
    </section>
  );
}

export default Hero;
