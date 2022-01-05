const handleChange = (model, setModel) => (event) => {
    const { target } = event;
    const value = target.type === 'checkbox' ? target.checked : target.value;
    const { name } = target;

    setModel({ ...model, [name]: value })
};

export default handleChange;