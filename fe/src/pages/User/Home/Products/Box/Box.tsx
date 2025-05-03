import { useNavigate } from "react-router-dom";
import "./Box.css";

interface ProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
}

interface BoxProps {
  product: ProductsProps;
  handleAddToCart: (product: ProductsProps, numberOfQuantity: number) => void;
}

const Box = ({ product, handleAddToCart }: BoxProps) => {
  const { discount, image: img, title, rating, price, salePrice } = product;
  const navigate = useNavigate();

  const handleSeeDetails = (titleProduct: string) => {
    navigate(`/product/${titleProduct}`);
  };

  return (
    <div className="product-box">
      <span>{discount}</span>
      <img src={img} alt="" onClick={() => handleSeeDetails(title)} />
      <h2>{title}</h2>
      <div className="rating">
        <i className="ri-star-fill"></i>
        <p>{rating}</p>
      </div>
      <div className="p-info">
        <div className="price">
          <p>${price}</p>
          <h3>${salePrice}</h3>
        </div>
        <a onClick={() => handleAddToCart(product, 1)}>
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
