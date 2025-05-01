import { JSX } from "react";
import "./Footer.css";
import img_logo from "../../assets/logo.png";
function Footer(): JSX.Element {
  return (
    <footer>
      <div className="footer-container container">
        <div className="footer-box">
          <h3>
            Product <span>Categories</span>
          </h3>
          <a href="#" className="footer-link">
            Camping Tent
          </a>
          <a href="#" className="footer-link">
            Backpack
          </a>
          <a href="#" className="footer-link">
            Flask And Mug
          </a>
          <a href="#" className="footer-link">
            Sleeping Bag
          </a>
        </div>
        <div className="footer-box footer-center">
          <a href="#" className="logo">
            <img src={img_logo} alt="" />
          </a>
          <p>"Professional equipment for unforgettable adventures"</p>
          <div className="social">
            <a href="#">
              <i className="ri-telegram-fill"></i>
            </a>
            <a href="#">
              <i className="ri-instagram-line"></i>
            </a>
            <a href="#">
              <i className="ri-whatsapp-line"></i>
            </a>
            <a href="#">
              <i className="ri-twitter-x-line"></i>
            </a>
          </div>
        </div>
        <div className="footer-box">
          <h3>
            Important <span>Links</span>
          </h3>
          <a href="#" className="footer-link">
            Blog
          </a>
          <a href="#" className="footer-link">
            Products
          </a>
          <a href="#" className="footer-link">
            About Us
          </a>
          <a href="#" className="footer-link">
            Contact Us
          </a>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
