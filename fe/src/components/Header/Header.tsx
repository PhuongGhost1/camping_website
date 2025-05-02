import { useEffect, useState } from "react";
import "./Header.css";
import img_logo from "../../assets/logo.png";

interface LinkProps {
  link: string;
  name: string;
}

interface SellingProductsProps {
  discount: string;
  image: string;
  title: string;
  rating: string;
  price: string;
  salePrice: string;
}

interface HeaderProps {
  carts: SellingProductsProps[];
  quanity: number;
  totalPriceOnCart: number;
}

const header: LinkProps[] = [
  {
    link: "#category",
    name: "Category",
  },
  {
    link: "#products",
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

const Header = ({ carts, quanity, totalPriceOnCart }: HeaderProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [isOpenCart, setIsOpenCart] = useState(false);
  const [quantityOnCart, setQuantityOnCart] = useState(quanity);
  const [cartItems, setCartItems] = useState<SellingProductsProps[]>([]);
  const [totalPrice, setTotalPrice] = useState(totalPriceOnCart);

  useEffect(() => {
    const handleScroll = () => {
      setIsOpen(false);
    };

    window.addEventListener("scroll", handleScroll);

    setCartItems(carts);

    const total = cartItems.reduce((acc, item) => {
      return acc + Number(item.price) * quantityOnCart;
    }, 0);
    setTotalPrice(total);
  }, [cartItems, quantityOnCart, totalPrice, carts]);

  const handleMenuToggle = () => {
    setIsOpen(!isOpen);
  };

  const handleCartToggle = () => {
    setIsOpenCart(!isOpenCart);
  };

  return (
    <header>
      <div className="nav container">
        <a href="#" className="logo">
          <img src={img_logo} alt="" />
        </a>
        <div className={"navbar" + (isOpen ? " open-navbar" : "")}>
          {header.map((item: LinkProps, index: number) => (
            <a key={index} href={item.link} className="nav-link">
              {item.name}
            </a>
          ))}
          <a href="#" className="login-btn mobile-login">
            Login / Register
          </a>
        </div>
        <div className="nav-right">
          <i className="ri-search-2-line"></i>
          <a href="#" className="login-btn">
            Login / Register
          </a>
          <div
            className={"menu-icon" + (isOpen ? " move" : "")}
            onClick={handleMenuToggle}
          >
            <div className="line1"></div>
            <div className="line2"></div>
            <div className="line3"></div>
          </div>
          <div className="cart-icon" onClick={handleCartToggle}>
            <i className="ri-shopping-basket-line"></i>
            <div className="basket-quantity">{quantityOnCart}</div>
          </div>
          <div className={"cart-dropdown" + (isOpenCart ? " open-cart" : "")}>
            <div className="header-cart">
              <p>Your Basket</p>
              <span onClick={() => setIsOpenCart(false)}>
                <i className="ri-close-line"></i>
              </span>
            </div>
            {cartItems.length > 0 ? (
              <>
                <div className="cart-list-container">
                  {cartItems.map(
                    (item: SellingProductsProps, index: number) => (
                      <div key={index} className="cart-item-row">
                        <div className="cart-item-img-wrapper">
                          <img src={item.image} alt={item.title} />
                          <div className="cart-item-quantity-wrapper">
                            <p>Qty</p>
                            <input
                              type="number"
                              value={quantityOnCart}
                              onChange={(e) =>
                                setQuantityOnCart(Number(e.target.value))
                              }
                              required
                            />
                          </div>
                        </div>
                        <div className="cart-item-details">
                          <div>
                            <h3>{item.title}</h3>
                            <p>$ {item.price} USD</p>
                          </div>
                          <button className="btn-remove">Remove</button>
                        </div>
                      </div>
                    )
                  )}
                </div>
                <div className="cart-footer">
                  <div>
                    <p>SubTotal</p>
                    <span>$ 0.00 USD</span>
                  </div>
                  <a href="#" className="btn">
                    Continue to checkout
                  </a>
                </div>{" "}
              </>
            ) : (
              <div className="empty-cart">
                <p>Your cart is empty</p>
                <a href="#">
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
