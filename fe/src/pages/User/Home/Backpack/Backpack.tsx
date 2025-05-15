import { JSX } from "react";
import "./Backpack.css";
import img_backpack_grid from "../../../../assets/backpack-grid.png";
import img_arrow_right from "../../../../assets/arrow-right.png";
import img_backpack from "../../../../assets/backpack.png";

function Backpack(): JSX.Element {
  return (
    <section className="backpack container" id="backpack">
      <div className="backpack-content">
        <img src={img_backpack_grid} alt="" className="backpack-grid" />
        <h2>
          Backpack Series <br />
          <span>Jack WolfSkin</span>
        </h2>
        <p>
          The Jack Wolfskin backpack series is designed for outdoor enthusiasts
          who demand the best in performance and durability. With a range of
          sizes and styles to choose from, these backpacks are perfect for any
          adventure, whether you're hiking, camping, or traveling.
        </p>
        <a href="/product/medium-bag" className="btn">
          <span>View Products</span>
          <img src={img_arrow_right} alt="" />
        </a>
      </div>
      <img src={img_backpack} alt="" className="backpack-img" />
    </section>
  );
}

export default Backpack;
