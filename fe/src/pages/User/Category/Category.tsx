import React, { useEffect, useState } from "react";
import { SellingProductsProps } from "../../../App";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import "./Category.css";
import Box from "../Home/Products/Box/Box";
import { useParams } from "react-router-dom";

interface CategoryLinkProps {
  name: string;
  link: string;
}

const categoryNameList: CategoryLinkProps[] = [
  {
    name: "Women",
    link: "women",
  },
  {
    name: "Men",
    link: "men",
  },
  {
    name: "Bottle",
    link: "bottle",
  },
  {
    name: "Footwear",
    link: "footwear",
  },
  {
    name: "Camping",
    link: "camping",
  },
  {
    name: "Accessories",
    link: "accessories",
  },
  {
    name: "Clothing",
    link: "clothing",
  },
  {
    name: "Bags",
    link: "bag",
  },
  {
    name: "Tents",
    link: "tent",
  },
];

interface CategoryProps {
  carts: SellingProductsProps[];
  quanity: number;
  totalPriceOnCart: number;
  onRemoveFromCart: (product: SellingProductsProps) => void;
  isOpenCartWhenAdd: boolean;
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  products: SellingProductsProps[];
  productBoxes: SellingProductsProps[];
  onAddToCart: (
    product: SellingProductsProps,
    numberOfQuantity: number
  ) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
}

const Category: React.FC<CategoryProps> = ({
  carts,
  quanity,
  totalPriceOnCart,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onUpdateCartQuantity,
  cartIconRef,
  products,
  onAddToCart,
}) => {
  const { name } = useParams<string>();
  const [isOpenSort, setIsOpenSort] = useState(false);
  const [sortedProducts, setSortedProducts] = useState<SellingProductsProps[]>(
    []
  );

  useEffect(() => {
    const filteredProducts = products.filter((product) =>
      product.category.some(
        (category) =>
          category.name.toLowerCase().trim() === name?.toLowerCase().trim()
      )
    );
    setSortedProducts(filteredProducts);
  }, [products, name]);

  const handleOpenSort = () => {
    setIsOpenSort(!isOpenSort);
  };

  const handleSort = (sortType: string) => {
    const sorted = [...sortedProducts];
    if (sortType === "nameAsc") {
      sorted.sort((a, b) => a.title.localeCompare(b.title));
    } else if (sortType === "nameDesc") {
      sorted.sort((a, b) => b.title.localeCompare(a.title));
    } else if (sortType === "priceAsc") {
      sorted.sort((a, b) => a.salePrice - b.salePrice);
    } else if (sortType === "priceDesc") {
      sorted.sort((a, b) => b.salePrice - a.salePrice);
    }
    setSortedProducts(sorted);
  };

  return (
    <>
      <Header
        carts={carts}
        quanity={quanity}
        totalPriceOnCart={totalPriceOnCart}
        onRemoveFromCart={onRemoveFromCart}
        isOpenCartWhenAdd={isOpenCartWhenAdd}
        onUpdateCartQuantity={onUpdateCartQuantity}
        cartIconRef={cartIconRef}
        sellingProducts={products}
      />

      <div className="product-category container">
        <div className="category-header">
          <div className="link-category">
            <a href="/shop-all-products">All Products</a>
            {categoryNameList.map((category, index) => (
              <a
                key={index}
                href={category.link}
                className={`category-link ${
                  name?.toLowerCase() === category.link ? "active" : ""
                }`}
              >
                {category.name}
              </a>
            ))}
          </div>
          <div className="sort-category">
            <button className="btn-sort" onClick={handleOpenSort}>
              Sort By{" "}
              <i
                className={`ri-arrow-down-s-line ${isOpenSort ? "rotate" : ""}`}
              ></i>
            </button>
            <div className={"sort-select" + (isOpenSort ? " open" : "")}>
              <p onClick={() => handleSort("nameAsc")}>
                Name <strong>A to Z</strong>
              </p>
              <p onClick={() => handleSort("nameDesc")}>
                Name <strong>Z to A</strong>
              </p>
              <p onClick={() => handleSort("priceAsc")}>
                Price <strong>Low to High</strong>
              </p>
              <p onClick={() => handleSort("priceDesc")}>
                Price <strong>High to Low</strong>
              </p>
            </div>
          </div>
        </div>
        <div className="product-list">
          {sortedProducts.map((product, index) => (
            <Box
              key={index}
              product={product}
              handleAddToCart={onAddToCart}
              cartIconRef={cartIconRef}
            />
          ))}
        </div>
      </div>

      <Footer />
    </>
  );
};

export default Category;
