import "./Selling.css";
import img_arrow_right from "../../../../assets/arrow-right.png";
import {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  Autoplay,
} from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css";
import "swiper/css/navigation";
import "swiper/css/pagination";
import "swiper/css/scrollbar";
import Box from "../Products/Box/Box";
import { ProductFromApi, UserProps } from "../../../../App";

interface SellingProps {
  sellingProducts: ProductFromApi[];
  onAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
  cartIconRef?: React.RefObject<HTMLDivElement>;
  user: UserProps | null;
}

const Selling = ({
  sellingProducts,
  onAddToCart,
  cartIconRef,
  user,
}: SellingProps) => {
  return (
    <section className="selling container" id="selling">
      <div className="selling-heading">
        <h2>Best Selling</h2>
        <a href="/shop-all-products" className="btn">
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
        {Array.isArray(sellingProducts) &&
          sellingProducts.map((product: ProductFromApi, index: number) => (
            <SwiperSlide key={index}>
              <Box
                product={product}
                handleAddToCart={onAddToCart}
                cartIconRef={cartIconRef}
                user={user}
              />
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
};

export default Selling;
