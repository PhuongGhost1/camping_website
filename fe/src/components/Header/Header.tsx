import React, { useEffect, useState } from "react";
import "./Header.css";
import img_logo from "../../assets/logo.png";
import { OrderItemProps, ProductFromApi, UserProps } from "../../App";
import Authentication from "../Authentication/Authentication";
import Profile from "../Profile/Profile";
import { useNavigate } from "react-router-dom";

interface LinkProps {
  link: string;
  name: string;
}

interface HeaderProps {
  carts: OrderItemProps[];
  quanity: number;
  totalPriceOnCart: number;
  onRemoveFromCart: (item: ProductFromApi) => void;
  isOpenCartWhenAdd: boolean;
  onUpdateCartQuantity: (product: ProductFromApi, quantity: number) => void;
  cartIconRef: React.RefObject<HTMLDivElement>;
  sellingProducts: ProductFromApi[];
  user: UserProps | null;
}

const header: LinkProps[] = [
  {
    link: "/category/bottle",
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

const placeholders = ["Name", "Country", "Email", "Product"];
const TYPING_SPEED = 100;
const PAUSE_DURATION = 1500;
const ERASE_SPEED = 50;

const Header = ({
  carts,
  quanity,
  totalPriceOnCart,
  onRemoveFromCart,
  isOpenCartWhenAdd,
  onUpdateCartQuantity,
  cartIconRef,
  sellingProducts,
  user,
}: HeaderProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [isOpenCart, setIsOpenCart] = useState(false);
  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [type, setType] = useState("Login");
  const [isOpenSearch, setIsOpenSearch] = useState(false);
  const [isOpenSearchResult, setIsOpenSearchResult] = useState(false);
  const [searchValue, setSearchValue] = useState("");
  const [searchResults, setSearchResults] = useState<ProductFromApi[]>([]);
  const isMobile = window.innerWidth <= 768;
  const [isOpenUserInfo, setIsOpenUserInfo] = useState(false);
  const navigate = useNavigate();
  const [displayText, setDisplayText] = useState("");
  const [placeholderIndex, setPlaceholderIndex] = useState(0);
  const [typing, setTyping] = useState(true);

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
    if (!user) {
      setIsOpenCart(false);
      navigate("/");
    } else {
      setIsOpenCart(!isOpenCart);
    }
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

    const filteredResults = sellingProducts.filter((item) =>
      item.name.toLowerCase().includes(value.toLowerCase())
    );

    setSearchResults(filteredResults);
    setIsOpenSearchResult(filteredResults.length > 0 && value.length > 0);
  };

  const handleOpenUserInfo = () => {
    setIsOpenUserInfo(!isOpenUserInfo);
  };

  useEffect(() => {
    let timeout: ReturnType<typeof setTimeout>;

    const fullText = placeholders[placeholderIndex];

    if (typing) {
      if (displayText.length < fullText.length) {
        timeout = setTimeout(() => {
          setDisplayText(fullText.slice(0, displayText.length + 1));
        }, TYPING_SPEED);
      } else {
        timeout = setTimeout(() => {
          setTyping(false);
        }, PAUSE_DURATION);
      }
    } else {
      if (displayText.length > 0) {
        timeout = setTimeout(() => {
          setDisplayText(fullText.slice(0, displayText.length - 1));
        }, ERASE_SPEED);
      } else {
        setTyping(true);
        setPlaceholderIndex((prev) => (prev + 1) % placeholders.length);
      }
    }

    return () => clearTimeout(timeout);
  }, [displayText, typing, placeholderIndex]);

  return (
    <header>
      {isLoginOpen && <Authentication closeLogin={closeLogin} type={type} />}
      {isOpenSearch && (
        <div className="background" onClick={handleCloseSearch}></div>
      )}

      {user && (
        <Profile
          user={user}
          handleOpenUserInfo={handleOpenUserInfo}
          isOpenUserInfo={isOpenUserInfo}
        />
      )}
      <>
        {isOpenSearch && (
          <div className="search-wrapper">
            <div className={`search-bar ${isOpenSearch ? "open-search" : ""}`}>
              <input
                type="text"
                placeholder={`Search For ${displayText}`}
                value={searchValue}
                onChange={(event) => handleInputChange(event)}
              />
              <i className="ri-close-line" onClick={handleCloseSearch}></i>
            </div>
            {isOpenSearchResult && (
              <div className="search-result-container">
                <div className="search-result-list">
                  {searchResults.map((item, index) => (
                    <a
                      href={`/product/${item.name
                        .toLowerCase()
                        .replace(/\s+/g, "-")}`}
                      key={index}
                      className="search-result-item"
                    >
                      {item.name}
                    </a>
                  ))}
                </div>
              </div>
            )}
          </div>
        )}
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
          {isMobile && !user && (
            <button className="login-btn mobile-login" onClick={openLogin}>
              Login / Register
            </button>
          )}
        </div>
        <div className="nav-right">
          <i className="ri-search-2-line" onClick={handleSearchToggle}></i>
          {user ? (
            <div className="user-info" onClick={handleOpenUserInfo}>
              <i className="ri-user-line"></i>
              <p>{user.name}</p>
            </div>
          ) : (
            <button className="login-btn" onClick={openLogin}>
              Login / Register
            </button>
          )}

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
                  {carts.map((item: OrderItemProps, index: number) => {
                    const product = item.product;
                    if (!product) return null;

                    return (
                      <div
                        key={index}
                        className={
                          "cart-item-row" + (item.removing ? " blink-out" : "")
                        }
                      >
                        <div className="cart-item-img-wrapper">
                          <img src={product.imageUrl} alt={product.name} />
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
                                onUpdateCartQuantity(product, numericValue);
                              }}
                            />
                          </div>
                        </div>
                        <div className="cart-item-details">
                          <div>
                            <h3>{product.name}</h3>
                            <p>$ {product.price} USD</p>
                          </div>
                          <button
                            className="btn-remove"
                            onClick={() => onRemoveFromCart(product)}
                          >
                            Remove
                          </button>
                        </div>
                      </div>
                    );
                  })}
                </div>
                <div className="cart-footer">
                  <div>
                    <p>SubTotal</p>
                    <span>$ {totalPriceOnCart.toFixed(2)} USD</span>
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
