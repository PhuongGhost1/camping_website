import { useRef, useState } from "react";
import { ProductFromApi } from "../../../../App";
import ProductDescription from "./ProductDescription/ProductDescription";
import "./ProductDetail.css";
import { animateAddToCart } from "../../../../helper/utilities/utilities";

interface ProductDetailProps {
  currentProduct: ProductFromApi;
  onAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
  cartIconRef?: React.RefObject<HTMLDivElement>;
}

const ProductDetail = ({
  currentProduct,
  onAddToCart,
  cartIconRef,
}: ProductDetailProps) => {
  const [currentQuantity, setCurrentQuantity] = useState<number>(1);
  const [isAdding, setIsAdding] = useState<boolean>(false);
  const imgRef = useRef<HTMLImageElement>(null);

  const handleAdded = () => {
    setIsAdding(true);

    setTimeout(() => {
      onAddToCart(currentProduct, currentQuantity);
      setIsAdding(false);

      if (imgRef?.current && cartIconRef?.current) {
        animateAddToCart(imgRef.current, cartIconRef.current);
      }
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
            <a href="/category/men">
              <div>Men</div>
            </a>
            <div className="breadcrumb-divider w-embed">
              <i className="ri-arrow-right-s-line"></i>
            </div>
            <a
              href={
                "/product/" +
                currentProduct.name.toLowerCase().replace(/\s+/g, "-")
              }
            >
              <div>{currentProduct.name}</div>
            </a>
          </div>
          <img
            ref={imgRef}
            src={currentProduct.imageUrl}
            alt={currentProduct.name}
          />
        </div>
        <div className="product-info">
          <h2>{currentProduct.name}</h2>
          <h2>$ {currentProduct.price.toFixed(2)} USD</h2>
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
            <div className="text-size-small">(5 starts) • 10 reviews</div>
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
