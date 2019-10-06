import React from "react";

// Stateless Functional Component
// Destructuring 'props' and using only totalCounters property
// Here one cannot use life cycle hooks
const NavBar = props => {
  console.log(`${NavBar.name} -> Rendered`, props);

  return (
    <nav className="navbar navbar-light bg-light">
      <a className="navbar-brand" href="#">
        Navbar{" "}
        <span className="badge badge-pill badge-secondary">
          {props.totalCounters}
        </span>
      </a>
    </nav>
  );
};

export default NavBar;
