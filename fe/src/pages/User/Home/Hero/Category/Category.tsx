import { JSX } from "react";
import "./Category.css";
import {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  Autoplay,
} from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";
import img_category_1 from "../../../../../assets/category-1.png";
import img_category_2 from "../../../../../assets/category-2.png";
import img_category_3 from "../../../../../assets/category-3.png";
import img_category_4 from "../../../../../assets/category-4.png";
import img_category_5 from "../../../../../assets/category-5.png";
import img_category_6 from "../../../../../assets/category-6.png";
import img_bottom_scroll from "../../../../../assets/bottom-scroll.png";
import img_category_bg from "../../../../../assets/category-bg.png";

interface CategoryBoxProps {
  img: string;
  title: string;
}

const boxes: CategoryBoxProps[] = [
  { img: img_category_1, title: "Torch" },
  { img: img_category_2, title: "Mug" },
  { img: img_category_3, title: "Chair" },
  { img: img_category_4, title: "Bag" },
  { img: img_category_5, title: "Camping Tent" },
  { img: img_category_6, title: "Sleeping Bag" },
];

function Category(): JSX.Element {
  return (
    <div className="category container" id="category">
      <a href="#products">
        <img src={img_bottom_scroll} alt="" className="scroll-bottom" />
      </a>
      <div className="category-content">
        <div className="img-wrapper">
          <span></span>
          <img src={img_category_bg} alt="" className="category-img-1" />
          <img src={img_category_bg} alt="" className="category-img-2" />
        </div>
        <h2>Product Categories</h2>
        <Swiper
          modules={[Navigation, Pagination, Scrollbar, A11y, Autoplay]}
          autoplay={{ delay: 2500, disableOnInteraction: false }}
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
            900: {
              slidesPerView: 4,
              spaceBetween: 20,
            },
          }}
          className="categorySwiper"
        >
          {boxes.map((box: CategoryBoxProps, index: number) => (
            <SwiperSlide key={index}>
              <div className="category-box">
                <img src={box.img} alt={box.title} />
                <h3>{box.title}</h3>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
        <div className="swiper-btn">
          <div className="swiper-button-prev"></div>
          <div className="swiper-button-next"></div>
        </div>
      </div>
    </div>
  );
}

export default Category;
