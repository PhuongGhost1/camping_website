import { JSX } from "react";
import "./Brand.css";
import img_brand_1 from "../../../../assets/brand-1.png";
import img_brand_2 from "../../../../assets/brand-2.png";
import img_brand_3 from "../../../../assets/brand-3.png";
import img_brand_4 from "../../../../assets/brand-4.png";
import img_brand_5 from "../../../../assets/brand-5.png";
import img_brand_6 from "../../../../assets/brand-6.png";
import img_brand_7 from "../../../../assets/brand-7.png";
import img_brand_8 from "../../../../assets/brand-8.png";

interface BrandProps {
  img: string;
}

const brands: BrandProps[] = [
  { img: img_brand_1 },
  { img: img_brand_2 },
  { img: img_brand_3 },
  { img: img_brand_4 },
  { img: img_brand_5 },
  { img: img_brand_6 },
  { img: img_brand_7 },
  { img: img_brand_8 },
];

function Brand(): JSX.Element {
  return (
    <section className="brands" id="brands">
      <h2 className="heading">Brands</h2>
      <div className="brand-container">
        <div className="brand-content container">
          {brands.map((brand: BrandProps, index: number) => (
            <div key={index} className="brand-box">
              <img src={brand.img} alt="" />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

export default Brand;
