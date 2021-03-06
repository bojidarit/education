import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import * as serviceWorker from "./serviceWorker";
import "bootstrap/dist/css/bootstrap.css";
import App from "./App";
// import TitledCounters from "./components/titledCounters";
// import List from "./components/listComponent";

ReactDOM.render(
  <App name="Demo Application" />,
  document.getElementById("root")
);
// ReactDOM.render(<TitledCounters />, document.getElementById("root"));
// ReactDOM.render(<List />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
