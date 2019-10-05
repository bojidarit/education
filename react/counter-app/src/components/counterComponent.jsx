import React, { Component } from "react";

class Counter extends Component {
  render() {
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
