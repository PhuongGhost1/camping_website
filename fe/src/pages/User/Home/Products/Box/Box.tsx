import { useNavigate } from "react-router-dom";
import "./Box.css";
import { animateAddToCart } from "../../../../../helper/utilities/utilities";
import { useRef } from "react";
import { ProductFromApi, UserProps } from "../../../../../App";

interface BoxProps {
  product: ProductFromApi;
  handleAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
  cartIconRef?: React.RefObject<HTMLDivElement>;
  user: UserProps | null;
}

const Box = ({ product, handleAddToCart, cartIconRef, user }: BoxProps) => {
  const { name, imageUrl, price } = product;
  const navigate = useNavigate();
  const imgRef = useRef<HTMLImageElement>(null);

  const handleSeeDetails = (titleProduct: string) => {
    navigate(`/product/${titleProduct}`);
  };

  const handleAddToCartClick = () => {
    handleAddToCart(product, 1);
    if (user && imgRef?.current && cartIconRef?.current) {
      animateAddToCart(imgRef.current, cartIconRef.current);
    }
  };

  return (
    <div className="product-box">
      <span>30%</span>
      <img
        ref={imgRef}
        src={imageUrl}
        alt=""
        onClick={() => handleSeeDetails(name)}
      />
      <h2>{name}</h2>
      <div className="rating">
        <i className="ri-star-fill"></i>
        <p>4.0</p>
      </div>
      <div className="p-info">
        <div className="price">
          <p>${price.toFixed(2)}</p>
          <h3>${price.toFixed(2)}</h3>
        </div>
        <a onClick={handleAddToCartClick}>
          <i className="ri-handbag-line"></i>
        </a>
      </div>
      <button className="btn-details" onClick={() => handleSeeDetails(name)}>
        See Details
      </button>
    </div>
  );
};

export default Box;
