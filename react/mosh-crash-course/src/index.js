// import React from "react";
// import ReactDOM from "react-dom";
// const element = <h1>Hello World...</h1>;
// ReactDOM.render(element, document.getElementById("root"));

// JavaScript for React Developers | Mosh
// https://youtu.be/NCwa_xi0Uuc

// var -> function scope
// let -> block scope
function loopDemo() {
  // for (var i = 0; i < 5; i++) {
  for (let i = 0; i < 5; i++) {
    console.log(`From loop -> ${i}`);
  }

  //console.log(`Outside loop -> ${i}`);
}
//loopDemo();

// const -> constant (block scope)
const PI = 3.14;
console.log(PI);
//PI = 3.1;
