import "./Box.css";

interface ProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
}

const Box = ({ product }: { product: ProductsProps }) => {
  const { discount, image: img, title, rating, price, salePrice } = product;
  return (
    <div className="product-box">
      <span>{discount}</span>
      <img src={img} alt="" />
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
        <a href="#">
          <i className="ri-handbag-line"></i>
        </a>
      </div>
    </div>
  );
};

export default Box;
