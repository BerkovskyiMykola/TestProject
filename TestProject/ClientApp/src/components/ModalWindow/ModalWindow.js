import React from 'react';
import { Modal, ModalHeader, ModalBody, FormGroup, Button } from "reactstrap";
import { Form } from '../FormComponents';


const ModalWindow = ({ modal, deactiveModal, message, children, textHeader, textButton, method }) => {

    const handleSubmit = (e) => {
        e.preventDefault();
        method();
    }

    return (
        <Modal isOpen={modal} toggle={deactiveModal}>
            <ModalHeader toggle={deactiveModal} >{textHeader}</ModalHeader>
            <ModalBody>
                <Form handleSubmit={handleSubmit} message={message}>
                    {children}
                    <FormGroup>
                        <Button block color="primary">{textButton}</Button>
                    </FormGroup>
                </Form>
            </ModalBody>
        </Modal>
    );
}

export default ModalWindow;