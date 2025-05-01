import { JSX } from "react";
import "./Selling.css";
import img_arrow_right from "../../../../assets/arrow-right.png";
import img_selling_1 from "../../../../assets/selling-1.png";
import img_selling_2 from "../../../../assets/selling-2.png";
import img_selling_3 from "../../../../assets/selling-3.png";
import img_selling_4 from "../../../../assets/selling-4.png";
import img_product_1 from "../../../../assets/product-1.png";
import {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  Autoplay,
} from "swiper/modules";

import { Swiper, SwiperSlide } from "swiper/react";

// Import Swiper styles
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";
import Box from "../Products/Box/Box";

interface SellingProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
}

const sellingProducts: SellingProductsProps[] = [
  {
    discount: "30%",
    image: img_selling_1,
    title: "Small Sun Headlight",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_selling_2,
    title: "Sprint Sunglasses",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_selling_3,
    title: "Sleeping Bag",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_selling_4,
    title: "Eletric Water Bottle",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
  {
    discount: "30%",
    image: img_product_1,
    title: "Camping Tent",
    rating: "4.5",
    price: "300.00",
    salePrice: "200.00",
  },
];

function Selling(): JSX.Element {
  return (
    <section className="selling container" id="selling">
      <div className="selling-heading">
        <h2>Best Selling</h2>
        <a href="#" className="btn">
          <span>View Products</span>
          <img src={img_arrow_right} alt="" />
        </a>
      </div>

      <Swiper
        modules={[Navigation, Pagination, Scrollbar, A11y, Autoplay]}
        autoplay={{ delay: 4500, disableOnInteraction: false }}
        pagination={{ el: ".swiper-pagination", clickable: true }}
        navigation={{
          nextEl: ".swiper-button-next",
          prevEl: ".swiper-button-prev",
        }}
        breakpoints={{
          280: {
            slidesPerView: 1,
            spaceBetween: 10,
          },
          510: {
            slidesPerView: 2,
            spaceBetween: 10,
          },
          750: {
            slidesPerView: 3,
            spaceBetween: 15,
          },
          920: {
            slidesPerView: 4,
            spaceBetween: 20,
          },
        }}
        className="selling-slider"
      >
        {sellingProducts.map((product: SellingProductsProps, index: number) => (
          <SwiperSlide key={index}>
            <Box product={product} />
          </SwiperSlide>
        ))}

        <div className="slides-control">
          <div className="swiper-pagination"></div>
          <div className="swiper-btn">
            <div className="swiper-button-prev"></div>
            <div className="swiper-button-next"></div>
          </div>
        </div>
      </Swiper>
    </section>
  );
}

export default Selling;
