import React, { useEffect, useState } from "react";
import "./Header.css";
import img_logo from "../../assets/logo.png";
import { SellingProductsProps } from "../../App";
import Authentication from "../Authentication/Authentication";

interface LinkProps {
  link: string;
  name: string;
}

interface HeaderProps {
  carts: SellingProductsProps[];
  quanity: number;
  totalPriceOnCart: number;
  onRemoveFromCart: (item: SellingProductsProps) => void;
  isOpenCartWhenAdd: boolean;
  onUpdateCartQuantity: (title: string, quantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
}

const header: LinkProps[] = [
  {
    link: "/category/men",
    name: "Category",
  },
  {
    link: "/shop-all-products",
    name: "Products",
  },
  {
    link: "#tips",
    name: "Blog",
  },
  {
    link: "#backpack",
    name: "About Us",
  },
  {
    link: "#",
    name: "Contact Us",
  },
];

const searchResult = [
  { name: "Bottle XL" },
  { name: "Bottle S" },
  { name: "Bottle L" },
  { name: "Bottle XXL" },
  { name: "Bottle M" },
];

const Header = ({
  carts,
  quanity,
  totalPriceOnCart,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onUpdateCartQuantity,
  cartIconRef,
}: HeaderProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [isOpenCart, setIsOpenCart] = useState(false);
  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [type, setType] = useState("Login");
  const [isOpenSearch, setIsOpenSearch] = useState(false);
  const [isOpenSearchResult, setIsOpenSearchResult] = useState(false);
  const [searchValue, setSearchValue] = useState("");
  const [searchResults, setSearchResults] = useState(searchResult);

  const openLogin = () => {
    setIsLoginOpen(true);
    setType("Login");
  };

  const closeLogin = () => {
    setIsLoginOpen(false);
  };

  useEffect(() => {
    const handleScroll = () => {
      setIsOpen(false);
    };

    window.addEventListener("scroll", handleScroll);

    if (isOpenCartWhenAdd) {
      setIsOpenCart(true);

      const timer = setTimeout(() => {}, 500);

      return () => clearTimeout(timer);
    }
  }, [carts, isOpenCartWhenAdd]);

  const handleMenuToggle = () => {
    setIsOpen(!isOpen);
  };

  const handleCartToggle = () => {
    setIsOpenCart(!isOpenCart);
  };

  const handleSearchToggle = () => {
    setIsOpenSearch(true);
    setIsOpenSearchResult(false);
    setSearchValue("");
  };

  const handleCloseSearch = () => {
    setIsOpenSearch(false);
    setIsOpenSearchResult(false);
    setSearchValue("");
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setSearchValue(value);

    const filteredResults = searchResult.filter((item) =>
      item.name.toLowerCase().includes(value.toLowerCase())
    );

    setSearchResults(filteredResults);
    setIsOpenSearchResult(filteredResults.length > 0 && value.length > 0);
  };

  return (
    <header>
      {isLoginOpen && <Authentication closeLogin={closeLogin} type={type} />}
      {isOpenSearch && (
        <div className="background" onClick={handleCloseSearch}></div>
      )}
      <>
        <div className="search-wrapper">
          <div className={`search-bar ${isOpenSearch ? "open-search" : ""}`}>
            <input
              type="text"
              placeholder="Search..."
              value={searchValue}
              onChange={(event) => handleInputChange(event)}
            />
            <i className="ri-close-line" onClick={handleCloseSearch}></i>
          </div>
          {isOpenSearchResult && (
            <div className="search-result-container">
              <div className="search-result-list">
                {searchResults.map((item, index) => (
                  <a href="#" key={index} className="search-result-item">
                    {item.name}
                  </a>
                ))}
              </div>
            </div>
          )}
        </div>
      </>
      <div className="nav container">
        <a href="/" className="logo">
          <img src={img_logo} alt="" />
        </a>
        <div className={"navbar" + (isOpen ? " open-navbar" : "")}>
          {header.map((item: LinkProps, index: number) => (
            <a key={index} href={item.link} className="nav-link">
              {item.name}
            </a>
          ))}
          <button className="login-btn mobile-login" onClick={openLogin}>
            Login / Register
          </button>
        </div>
        <div className="nav-right">
          <i className="ri-search-2-line" onClick={handleSearchToggle}></i>
          <button className="login-btn" onClick={openLogin}>
            Login / Register
          </button>

          <div
            className={"menu-icon" + (isOpen ? " move" : "")}
            onClick={handleMenuToggle}
          >
            <div className="line1"></div>
            <div className="line2"></div>
            <div className="line3"></div>
          </div>
          <div
            ref={cartIconRef}
            className="cart-icon"
            onClick={handleCartToggle}
          >
            <i className="ri-shopping-basket-line"></i>
            <div className="basket-quantity">{quanity}</div>
          </div>
          <div className={"cart-dropdown" + (isOpenCart ? " open-cart" : "")}>
            <div className="header-cart">
              <p>Your Basket</p>
              <span
                onClick={() => {
                  setIsOpenCart(false);
                }}
              >
                <i className="ri-close-line"></i>
              </span>
            </div>
            {carts.length > 0 ? (
              <>
                <div className="cart-list-container">
                  {carts.map((item: SellingProductsProps, index: number) => (
                    <div
                      key={index}
                      className={
                        "cart-item-row" + (item.removing ? " blink-out" : "")
                      }
                    >
                      <div className="cart-item-img-wrapper">
                        <img src={item.image} alt={item.title} />
                        <div className="cart-item-quantity-wrapper">
                          <p>Qty</p>
                          <input
                            type="number"
                            min={1}
                            value={item.quantity || 1}
                            onChange={(e) => {
                              const value = e.target.value.replace(
                                /[^0-9]/g,
                                ""
                              );
                              const numericValue = Math.max(1, Number(value));
                              onUpdateCartQuantity(item.title, numericValue);
                            }}
                          />
                        </div>
                      </div>
                      <div className="cart-item-details">
                        <div>
                          <h3>{item.title}</h3>
                          <p>$ {item.salePrice} USD</p>
                        </div>
                        <button
                          className="btn-remove"
                          onClick={() => onRemoveFromCart(item)}
                        >
                          Remove
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
                <div className="cart-footer">
                  <div>
                    <p>SubTotal</p>
                    <span>$ {totalPriceOnCart} USD</span>
                  </div>
                  <a href="/checkout" className="btn">
                    Continue to checkout
                  </a>
                </div>
              </>
            ) : (
              <div className="empty-cart">
                <p>Your cart is empty</p>
                <a href="/shop-all-products">
                  <button>Go Shop</button>
                </a>
              </div>
            )}
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
