import { JSX } from "react";
import "./Tips.css";
import img_blog_1 from "../../../../assets/blog-1.png";
import img_blog_2 from "../../../../assets/blog-2.png";
import img_blog_3 from "../../../../assets/blog-3.png";

interface BlogBoxProps {
  imgSrc?: string;
  category?: string;
  title: string;
  readTime?: string;
}

const blogBoxes: BlogBoxProps[] = [
  {
    imgSrc: img_blog_1,
    category: "Category",
    title:
      "Checklist of important and necessary supplies for camping and travel",
    readTime: "5 minutes read",
  },
  {
    imgSrc: img_blog_2,
    category: "Category",
    title: "The best camping gear for your next adventure",
    readTime: "25 minutes read",
  },
];

const posts: BlogBoxProps[] = [
  {
    imgSrc: img_blog_3,
    title: "Checklist of important camping supplies",
    readTime: "5 minutes read",
  },
];

const tips: BlogBoxProps[] = [
  {
    title: "Important tips for camping in winter",
  },
  {
    title: "Introducing the best ecocamps in Iran",
  },
  {
    title: "Guide to buying the best camping tent",
  },
  {
    title: "The rechargeable and battery fixed heads",
  },
];

function Tips(): JSX.Element {
  return (
    <section className="tips" id="tips">
      <h2 className="heading">Camping Tips</h2>
      <div className="tips-content container">
        <div className="blogs">
          {blogBoxes.map((blogBox: BlogBoxProps, index: number) => (
            <div className="blog-box" key={index}>
              <img src={blogBox.imgSrc} alt="" />
              <div className="blog-info">
                <span>{blogBox.category}</span>
                <h3>{blogBox.title}</h3>
                <div className="read">
                  <i className="ri-time-line"></i>
                  <p>{blogBox.readTime}</p>
                </div>
              </div>
            </div>
          ))}
        </div>

        {posts.map((post: BlogBoxProps, index: number) => (
          <div className="single-post" key={index}>
            <img src={post.imgSrc} alt="" />
            <div className="single-post-text">
              <div className="post-data">
                <h2>{post.title}</h2>
                <div className="read">
                  <i className="ri-time-line"></i>
                  <span>{post.readTime}</span>
                </div>
              </div>
              <a href="#">
                <i className="ri-arrow-right-up-line"></i>
              </a>
            </div>
          </div>
        ))}

        <div className="links">
          {tips.map((tip: BlogBoxProps, index: number) => (
            <div className="link-box" key={index}>
              <h3>{tip.title}</h3>
              <a href="#">
                <i className="ri-arrow-right-up-line"></i>
              </a>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

export default Tips;
