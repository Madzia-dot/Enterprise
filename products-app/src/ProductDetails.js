import React from 'react';
import { useParams, Link } from 'react-router-dom';

function ProductDetails({ products }) {
    const { id } = useParams();
    const product = products.find(p => p.id === parseInt(id));

    if (!product) {
        return null;
    }

    return (
        <div>
            <h1>{product.title}</h1>
            <p>Category: {product.category}<br />
               Brand: {product.brand}<br />
               Description: {product.description}<br />
               Price: ${product.price}</p>
            <img src={product.thumbnail} alt={product.title} />
            <br />
            <Link to="/">Back to product list</Link>
        </div>
    );
}

export default ProductDetails;