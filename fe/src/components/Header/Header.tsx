import { JSX, useEffect, useState } from "react";
import "./Header.css";
import img_logo from "../../assets/logo.png";

interface HeaderProps {
  link: string;
  name: string;
}

const header: HeaderProps[] = [
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

function Header(): JSX.Element {
  const [isOpen, setIsOpen] = useState(false);

  const handleMenuToggle = () => {
    setIsOpen(!isOpen);
  };

  useEffect(() => {
    const handleScroll = () => {
      setIsOpen(false);
    };

    window.addEventListener("scroll", handleScroll);
  }, []);
  return (
    <header>
      <div className="nav container">
        <a href="#" className="logo">
          <img src={img_logo} alt="" />
        </a>
        <div className={"navbar" + (isOpen ? " open-navbar" : "")}>
          {header.map((item: HeaderProps, index: number) => (
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
        </div>
      </div>
    </header>
  );
}

export default Header;
