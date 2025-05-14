import React, { useEffect, useState } from "react";
import {
  CategoryProductProps,
  OrderItemProps,
  ProductFromApi,
} from "../../../App";
import Footer from "../../../components/Footer/Footer";
import Header from "../../../components/Header/Header";
import "./Category.css";
import Box from "../Home/Products/Box/Box";
import { useParams } from "react-router-dom";
import { ApiGateway } from "../../../services/api/ApiService";

interface CategoryProps {
  carts: OrderItemProps[];
  quanity: number;
  totalPriceOnCart: number;
  onRemoveFromCart: (product: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  products: ProductFromApi[];
  productBoxes: ProductFromApi[];
  onAddToCart: (product: ProductFromApi, numberOfQuantity: number) => void;
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
  const [sortedProducts, setSortedProducts] = useState<ProductFromApi[]>([]);
  const [categoryNameList, setCategoryNameList] = useState<
    CategoryProductProps[]
  >([]);

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      const data = await ApiGateway.getAllCategories<CategoryProductProps[]>();
      setCategoryNameList(data || []);
    } catch (error) {
      console.error("Error fetching categories:", error);
      setCategoryNameList([]);
    }
  };

  useEffect(() => {
    const filteredProducts = products.filter((product) => {
      const productCategoryName = product.category?.name?.toLowerCase().trim();
      const routeName = name?.toLowerCase().trim();
      return productCategoryName === routeName;
    });

    setSortedProducts(filteredProducts);
  }, [products, name]);

  const handleOpenSort = () => {
    setIsOpenSort(!isOpenSort);
  };

  const handleSort = (sortType: string) => {
    const sorted = [...sortedProducts];
    if (sortType === "nameAsc") {
      sorted.sort((a, b) => a.name.localeCompare(b.name));
    } else if (sortType === "nameDesc") {
      sorted.sort((a, b) => b.name.localeCompare(a.name));
    } else if (sortType === "priceAsc") {
      sorted.sort((a, b) => a.price - b.price);
    } else if (sortType === "priceDesc") {
      sorted.sort((a, b) => b.price - a.price);
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
            {Array.isArray(categoryNameList) &&
              categoryNameList.map((category) => (
                <a
                  key={category.id}
                  href={`/category/${category.name.toLowerCase()}`}
                  className={`category-link ${
                    name?.toLowerCase() === category.name.toLowerCase()
                      ? "active"
                      : ""
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
