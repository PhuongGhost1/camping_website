import { useNavigate } from "react-router-dom";
import "./Box.css";
import { animateAddToCart } from "../../../../../helper/utilities/utilities";
import { useRef } from "react";

interface ProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: number;
  salePrice: number;
  category: string;
}

interface BoxProps {
  product: ProductsProps;
  handleAddToCart: (product: ProductsProps, numberOfQuantity: number) => void;
  cartIconRef?: React.RefObject<HTMLDivElement>;
}

const Box = ({ product, handleAddToCart, cartIconRef }: BoxProps) => {
  const { discount, image: img, title, rating, price, salePrice } = product;
  const navigate = useNavigate();
  const imgRef = useRef<HTMLImageElement>(null);

  const handleSeeDetails = (titleProduct: string) => {
    navigate(`/product/${titleProduct}`);
  };

  const handleAddToCartClick = () => {
    handleAddToCart(product, 1);
    if (imgRef?.current && cartIconRef?.current) {
      animateAddToCart(imgRef.current, cartIconRef.current);
    }
  };

  return (
    <div className="product-box">
      <span>{discount}</span>
      <img
        ref={imgRef}
        src={img}
        alt=""
        onClick={() => handleSeeDetails(title)}
      />
      <h2>{title}</h2>
      <div className="rating">
        <i className="ri-star-fill"></i>
        <p>{rating}</p>
      </div>
      <div className="p-info">
        <div className="price">
          <p>${price.toFixed(2)}</p>
          <h3>${salePrice.toFixed(2)}</h3>
        </div>
        <a onClick={handleAddToCartClick}>
          <i className="ri-handbag-line"></i>
        </a>
      </div>
      <button className="btn-details" onClick={() => handleSeeDetails(title)}>
        See Details
      </button>
    </div>
  );
};

export default Box;
