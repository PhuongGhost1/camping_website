export function animateAddToCart(
  imgRef: HTMLImageElement,
  cartIconRef: HTMLDivElement
) {
  const cartRect = cartIconRef.getBoundingClientRect();
  const rect = imgRef.getBoundingClientRect();

  const flyingItem = document.createElement("div");
  flyingItem.classList.add("flying-item");
  flyingItem.style.cssText = `
        position: fixed;
    z-index: 1000;
    width: 150px;
    height: 150px;
    background-image: url('${imgRef.src}');
    background-size: cover;
    border-radius: 50%;
    left: ${rect.left}px;
    top: ${rect.top}px;
    transition: all 1.5s ease-in-out;
    pointer-events: none;
    `;

  document.body.appendChild(flyingItem);

  setTimeout(() => {
    flyingItem.style.transform = `scale(0.3)`;
    flyingItem.style.left = `${cartRect.left + cartRect.width / 2 - 25}px`;
    flyingItem.style.top = `${cartRect.top + cartRect.height / 2 - 25}px`;
    flyingItem.style.opacity = "0";
  }, 50);

  setTimeout(() => {
    document.body.removeChild(flyingItem);
  }, 1600);
}

export function sanitize(str: string) {
  return str.replace(/\s+/g, "");
}
