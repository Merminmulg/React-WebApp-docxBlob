import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            file: null,
            email: '',
            loading: false,
            result: null,
            error: null,
        };
    }

    handleSubmit = async (event) => { 
        this.setState({ loading: true })
        event.preventDefault();

        // Получение данных из формы
        const formData = new FormData();
        formData.append('file', event.target.file.files[0]);

        try {
            const response = await fetch('api/files/uploadfile?email=' + event.target.email.value, {
                method: 'POST',
                body: formData,
            });
            console.log(response)
            if (response.ok) {
                this.setState({ loading: false})
                this.setState({ result: response.status, error: null });
                console.log('File uploaded');
            } else {
                this.setState({ loading: false })
                this.setState({ error: 'Произошла ошибка при загрузке файла.', result: null });
                console.error('Uploading error');
            }
        } catch (error) {
            this.setState({ loading: false })
            this.setState({ error: 'Произошла сетевая ошибка.', result: null });
            console.error('Network error:', error);
        }
    }

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit} encType="multipart/form-data">
                    <input type="file" name="file" />
                    <input type="text" name="email" />
                    <input type="submit" value="Submit" />
                </form>
                {this.state.loading && <p>Loading...</p>}
                {this.state.result && <p>File successfuly uploaded. Result: {this.state.result}</p>}
                {this.state.error && <p>Error: {this.state.error}</p>}
            </div>
        )
    }
}
