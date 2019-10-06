import React, { Component } from "react";
import Counter from "./counter";

class Counters extends Component {
  render() {
    console.log(`${this.constructor.name} -> ${this.render.name}`, this.props);

    const {
      counters,
      onReset,
      onDecrement,
      onIncrement,
      onDelete
    } = this.props;

    return (
      <div>
        <button onClick={onReset} className="btn btn-primary btn-new m-2">
          Reset
        </button>
        {counters.map(counter => (
          <Counter
            key={counter.id}
            onDelete={onDelete}
            onIncrement={onIncrement}
            onDecrement={onDecrement}
            counter={counter}
          />
        ))}
      </div>
    );
  }
}

export default Counters;
