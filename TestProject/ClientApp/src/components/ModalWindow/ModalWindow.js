import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody } from "reactstrap";
import { Form } from '../FormComponents';


const ModalWindow = ({ modal, deactiveModal, message, children, textHeader, textButton, method }) => {

    const [form, setForm] = useState(null);
    const [checkBtn, setCheckBtn] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();

        form.validateAll();

        if (checkBtn.context._errors.length === 0) {
            method();
        }

    }

    return (
        <Modal isOpen={modal} toggle={deactiveModal}>
            <ModalHeader toggle={deactiveModal} >{textHeader}</ModalHeader>
            <ModalBody>
                <Form handleSubmit={handleSubmit} setForm={setForm}
                    message={message} setCheckBtn={setCheckBtn}>
                    {children}
                    <div className="form-group">
                        <button className="btn btn-primary btn-block">{textButton}</button>
                    </div>
                </Form>
            </ModalBody>
        </Modal>
    );
}

export default ModalWindow;