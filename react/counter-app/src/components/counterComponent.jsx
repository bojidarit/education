import React, { Component } from "react";

class Counter extends Component {
  state = {
    value: this.props.value
  };

  // Bind Event-Handler
  // constructor() {
  //   super();
  //   this.handleIncrement = this.handleIncrement.bind(this);
  // }

  // Experimental binding with arrow function
  handleIncrement = product => {
    //console.log("Increment Clicked", this);
    this.setState({ value: this.state.value + 1 });
  };

  render() {
    return (
      <React.Fragment>
        {this.props.children}
        <span className={this.getBargeClasses()}>{this.formatCount()}</span>
        <button
          onClick={() => this.handleIncrement({ id: 777 })}
          className="btn btn-secondary btn-sm"
        >
          Increment
        </button>
      </React.Fragment>
    );
  }

  getBargeClasses() {
    return `badge m-2 badge-${this.state.value === 0 ? "warning" : "primary"}`;
  }

  formatCount() {
    const { value: count } = this.state; // object destructuring
    return count === 0 ? "Zero" : count;
  }
}

export default Counter;
