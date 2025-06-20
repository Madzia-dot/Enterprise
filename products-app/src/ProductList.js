import React, { useState, useEffect } from 'react';
import axios from 'axios';
import ProductItem from './ProductItem';


function ProductList({ products }) {
    const [filter, setFilter] = useState('');

    return (
        <div>
            <h1>List of products</h1>
            <label>Filter by product title: </label>
            <input
                type="text"
                value={filter}
                onChange={e => setFilter(e.target.value)}
            />
            <ul>
                {products
                    .filter(product => product.title.toLowerCase().includes(filter.toLowerCase()))
                    .map(product => (
                        <ProductItem key={product.id} product={product} />
                    ))}
            </ul>
        </div>
    );
}

export default ProductList;