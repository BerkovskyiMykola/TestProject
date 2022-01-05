import React from 'react';

const ListItem = ({ columns, item, index, action }) => {

    return (
        <tr>
            <td>{index + 1}</td>
            {columns.map((x, index) => 
                <td key={index}>{item[x]}</td>
            )}
            {action}
        </tr>
    );
}

export default ListItem;