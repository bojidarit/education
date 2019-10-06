import React, { Component } from "react";

class Counter extends Component {
  logMethodCall(methodName) {
    console.log(
      `${this.constructor.name} ${this.props.counter.id} with value = ${this.props.counter.value} -> ${methodName}`
    );
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.counter.value !== this.props.counter.value) {
      // Here one can make AJAX calls to get new data from the server...
      this.logMethodCall(this.componentDidUpdate.name + " with data update");
      console.log(
        `Counter ${this.props.counter.id} value changed from ${prevProps.counter.value} to ${this.props.counter.value}`
      );
    } else {
      this.logMethodCall(this.componentDidUpdate.name);
    }
  }

  // Here one can remove timer or listeners before the component is removed to prevent memory leaks!
  componentWillUnmount() {
    this.logMethodCall(this.componentWillUnmount.name);
  }

  render() {
    this.logMethodCall(this.render.name);

    return (
      <div>
        <span className={this.getBargeClasses()}>{this.formatCount()}</span>

        <button
          onClick={() => this.props.onIncrement(this.props.counter)}
          className="btn btn-secondary btn-sm"
        >
          ++
        </button>

        <button
          onClick={() => this.props.onDecrement(this.props.counter)}
          className="btn btn-secondary btn-sm m-2"
        >
          --
        </button>

        <button
          onClick={() => this.props.onDelete(this.props.counter.id)}
          className="btn btn-danger btn-sm m-2"
        >
          Delete
        </button>
      </div>
    );
  }

  getBargeClasses() {
    return `badge m-2 badge-${
      this.props.counter.value === 0 ? "warning" : "primary"
    }`;
  }

  formatCount() {
    const { value: count } = this.props.counter; // object destructuring
    return count === 0 ? "Zero" : count;
  }
}

export default Counter;
