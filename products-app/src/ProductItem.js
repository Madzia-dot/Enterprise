import React from 'react';
import { Link } from 'react-router-dom';

function ProductItem({ product }) {
    return (
        <li>
            <Link to={`/details/${product.id}`}>
                {product.title}
            </Link> ({product.brand})
        </li>
    );
}

export default ProductItem;