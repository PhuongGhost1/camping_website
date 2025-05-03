import { useState } from "react";
import { SellingProductsProps } from "../../../../App";
import ProductDescription from "./ProductDescription/ProductDescription";
import "./ProductDetail.css";

interface ProductDetailProps {
  title: string;
  currentProduct: SellingProductsProps;
  onAddToCart: (
    product: SellingProductsProps,
    numberOfQuantity: number
  ) => void;
}

const ProductDetail = ({
  title,
  currentProduct,
  onAddToCart,
}: ProductDetailProps) => {
  const [currentQuantity, setCurrentQuantity] = useState<number>(1);
  const [isAdding, setIsAdding] = useState<boolean>(false);

  const handleAdded = () => {
    setIsAdding(true);

    setTimeout(() => {
      onAddToCart(currentProduct, currentQuantity);
      setIsAdding(false);
    }, 1000);
  };

  return (
    <section className="product-details overflow-hidden">
      <div className="padding-global container grid">
        <div className="product-content-top">
          <div className="link-head">
            <a href="#">
              <div>New In</div>
            </a>
            <div className="breadcrumb-divider w-embed">
              <i className="ri-arrow-right-s-line"></i>
            </div>
            <a href="#">
              <div>Men</div>
            </a>
            <div className="breadcrumb-divider w-embed">
              <i className="ri-arrow-right-s-line"></i>
            </div>
            <a href={"/product/" + title}>
              <div>{title}</div>
            </a>
          </div>
          <img src={currentProduct.image} alt={currentProduct.title} />
        </div>
        <div className="product-info">
          <h2>{currentProduct.title}</h2>
          <h2>$ {currentProduct.salePrice} USD</h2>
          <div className="review-product">
            <div className="rating-wrapper">
              <div className="rating-icon">
                <i className="ri-star-fill"></i>
              </div>
              <div className="rating-icon">
                <i className="ri-star-fill"></i>
              </div>
              <div className="rating-icon">
                <i className="ri-star-fill"></i>
              </div>
              <div className="rating-icon">
                <i className="ri-star-fill"></i>
              </div>
              <div className="rating-icon">
                <i className="ri-star-half-fill"></i>
              </div>
            </div>
            <div className="text-size-small">
              ({currentProduct.rating} starts) • 10 reviews
            </div>
          </div>
          <a
            href="https://webflow.com/apps/detail/supersparks"
            target="_blank"
            className="text-size-tiny"
          >
            Add reviews to your Webflow ecommerce project
          </a>
          <div className="text-size-tiny">
            Aaron Grieve is in no way affiliated with Supersparks team or
            product.
          </div>

          <div className="quanity-product">
            <p>Quanity</p>
            <input
              type="number"
              min={1}
              value={currentQuantity}
              onChange={(e) => {
                const value = e.target.value.replace(/[^0-9]/g, "");
                const numericValue = Math.max(1, Number(value));
                setCurrentQuantity(numericValue);
              }}
            />
          </div>
          <button className="btn" onClick={handleAdded} disabled={isAdding}>
            {isAdding ? "Adding To Cart..." : "Add to Cart"}
          </button>
        </div>
        <ProductDescription />
      </div>
    </section>
  );
};

export default ProductDetail;
