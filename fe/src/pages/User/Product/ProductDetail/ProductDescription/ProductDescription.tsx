import { useState } from "react";
import "./ProductDescription.css";

function ProductDescription() {
  const [activeTab, setActiveTab] = useState("Details");
  return (
    <div className="product-description">
      <p>
        The TerraVenture Waterproof Pro Jacket is your all-weather,
        all-adventure companion – ready to face whatever Mother Nature throws at
        you. Enjoy the great outdoors in comfort and style.
      </p>
      <div
        data-easing="ease"
        data-duration-in="300"
        data-duration-out="100"
        className="product-header-tabs"
      >
        <div className="product-header-tab">
          {["Details", "Shipping", "Returns"].map((tab, index) => (
            <a
              key={tab}
              data-w-tab={tab}
              className={`product-header-tab-link ${
                activeTab === tab ? "w--current" : ""
              }`}
              id={`w-tabs-0-data-w-tab-${tab}`}
              href={`#w-tabs-0-data-w-pane-${index}`}
              role="tab"
              aria-controls={`w-tabs-0-data-w-pane-${index}`}
              aria-selected={activeTab === tab}
              onClick={() => setActiveTab(tab)}
            >
              <div>{tab}</div>
            </a>
          ))}
        </div>
        <div className="product-header-tab-content">
          {activeTab === "Details" && (
            <div className="product-header-tab-content-item">
              <div className="margin-top-small">
                <div className="w-richtext">
                  <p>
                    <strong>100% Waterproof Material:</strong> Engineered with
                    our top-tiered Triple Seal Tech waterproof fabric, ensuring
                    complete protection from rain and wet weather conditions.
                  </p>
                  <p>
                    <strong>Windproof:</strong> Effective in sealing out winds,
                    it offers an additional layer of protection against harsh
                    climates.
                  </p>
                  <p>
                    <strong>Breathable Design:</strong> Featuring our AeroFlow
                    technology, this jacket prevents overheating and
                    perspiration build-up, ensuring maximum comfort during long
                    activities.
                  </p>
                  <p>
                    <strong>Fully Sealed Seams:</strong> Every seam is fully
                    sealed, offering an extra layer of defense against the
                    leakage of water.
                  </p>
                  <p>
                    <strong>Lightweight and Compact:</strong> Even with its
                    robust protection, our jacket maintains a light weight and
                    can be easily packed into its pouch, perfect for
                    backpacking, hiking, and travel.
                  </p>
                  <p>
                    <strong>Durable:</strong> Crafted with wear-resistant
                    materials that withstand the test of time, ensuring
                    longevity even in heavy use.
                  </p>
                  <p>
                    <strong>Adjustable Hood and Cuffs:</strong> Equipped with an
                    adjustable, stow-away hood and Velcro cuffs for a custom fit
                    and optimal protection against the elements.
                  </p>
                  <p>
                    <strong>Roomy Pockets:</strong> Features two zippered hand
                    pockets and an internal pocket, providing secure storage for
                    your essentials.
                  </p>
                  <p>
                    <strong>YKK Zippers:</strong> High-quality, industry-leading
                    YKK zippers provide added water resistance and durability.
                  </p>
                  <p>
                    <strong>Reflective Details:</strong> Ensures increased
                    visibility and safety during low-light outdoor adventures.
                  </p>
                  <p>
                    <strong>Eco-friendly:</strong> Made from 100% recycled
                    materials, supporting our commitment to sustainability
                    without compromising performance.
                  </p>
                  <p>
                    <strong>Easy Care:</strong> Machine-washable and
                    quick-drying, ensuring easy care and maintenance.
                  </p>
                </div>
              </div>
            </div>
          )}
          {activeTab === "Shipping" && (
            <div className="product-header-tab-content-item">
              <div className="margin-top-small">
                <p>
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                  Suspendisse varius enim in eros elementum tristique. Duis
                  cursus, mi quis viverra ornare, eros dolor interdum nulla, ut
                  commodo diam libero vitae erat. Aenean faucibus nibh et justo
                  cursus id rutrum lorem imperdiet. Nunc ut sem vitae risus
                  tristique posuere.
                </p>
              </div>
            </div>
          )}
          {activeTab === "Returns" && (
            <div className="product-header-tab-content-item">
              <div className="margin-top-small">
                <p>
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                  Suspendisse varius enim in eros elementum tristique. Duis
                  cursus, mi quis viverra ornare, eros dolor interdum nulla, ut
                  commodo diam libero vitae erat. Aenean faucibus nibh et justo
                  cursus id rutrum lorem imperdiet. Nunc ut sem vitae risus
                  tristique posuere.
                </p>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default ProductDescription;
