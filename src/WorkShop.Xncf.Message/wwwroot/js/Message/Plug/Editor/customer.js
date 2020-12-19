

class MyUploadAdapter {
    constructor(loader) {
        this.loader = loader;
    }

    // Initializes the XMLHttpRequest object using the URL passed to the constructor.
    _initRequest() {
        const xhr = this.xhr = new XMLHttpRequest();

        // Note that your request may look different. It is up to you and your editor
        // integration to choose the right communication channel. This example uses
        // a POST request with JSON as a data structure but your configuration
        // could be different.
        xhr.open('POST', 'http://example.com/image/upload/path', true);
        xhr.responseType = 'json';
    }

    // Initializes XMLHttpRequest listeners.
    _initListeners(resolve, reject, file) {
        const xhr = this.xhr;
        const loader = this.loader;
        const genericErrorText = `Couldn't upload file: ${file.name}.`;

        xhr.addEventListener('error', () => reject(genericErrorText));
        xhr.addEventListener('abort', () => reject());
        xhr.addEventListener('load', () => {
            const response = xhr.response;

            // This example assumes the XHR server's "response" object will come with
            // an "error" which has its own "message" that can be passed to reject()
            // in the upload promise.
            //
            // Your integration may handle upload errors in a different way so make sure
            // it is done properly. The reject() function must be called when the upload fails.
            if (!response || response.error) {
                return reject(response && response.error ? response.error.message : genericErrorText);
            }

            // If the upload is successful, resolve the upload promise with an object containing
            // at least the "default" URL, pointing to the image on the server.
            // This URL will be used to display the image in the content. Learn more in the
            // UploadAdapter#upload documentation.
            resolve({
                default: response.url
            });
        });

        // Upload progress when it is supported. The file loader has the #uploadTotal and #uploaded
        // properties which are used e.g. to display the upload progress bar in the editor
        // user interface.
        if (xhr.upload) {
            xhr.upload.addEventListener('progress', evt => {
                if (evt.lengthComputable) {
                    loader.uploadTotal = evt.total;
                    loader.uploaded = evt.loaded;
                }
            });
        }
    }

    // Prepares the data and sends the request.
    _sendRequest(file) {
        // Prepare the form data.
        const data = new FormData();

        data.append('upload', file);

        // Important note: This is the right place to implement security mechanisms
        // like authentication and CSRF protection. For instance, you can use
        // XMLHttpRequest.setRequestHeader() to set the request headers containing
        // the CSRF token generated earlier by your application.

        // Send the request.
        this.xhr.send(data);
    }

    // Starts the upload process.
    upload() {
        return this.loader.file
            .then(file => new Promise((resolve, reject) => {
                debugger
                this._initRequest();
                this._initListeners(resolve, reject, file);
                this._sendRequest(file);
            }));
    }

    // Aborts the upload process.
    abort() {
        if (this.xhr) {
            this.xhr.abort();
        }
    }
}

function MyCustomUploadAdapterPlugin(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
        // Configure the URL to the upload script in your back-end here!
        return new MyUploadAdapter(loader);
    };
}

Vue.component('ckeditor', {
    // 声明 props
    props: ['content'],
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `
  <!-- 富文本编辑器组件 -->
   <div class="goods-editor">
        <!-- 工具栏容器 -->
        <div id="toolbar-container"></div>
        <!-- 编辑器容器 -->
        <div id="editor">
          <p>{{ content }}</p>
        </div>
      </div>`,
    name: 'ckeditor',
    data() {
        return {
            editor: null, // 编辑器实例
            loader: null,
            debug: false,
            imgUploadUrl:'https://htlx.milisx.xyz/api/v1/common/editorupload',
        };
    },
    mounted() {
        let that = this
        that.init()
        var url = window.location.href;
        if (url.indexOf('htlx') > -1) {
            that.imgUploadUrl = 'https://htlx.milisx.xyz/api/v1/common/editorupload';
        } else {
            that.imgUploadUrl = 'https://localhost:44311/api/v1/common/editorupload';
        }
    },
    methods: {
        init() {
            let that = this
            let tableConfig = {
                contentToolbar: ['tableRow', 'tableColumn', 'mergeTableCells']
            };
            ClassicEditor
                .create(document.querySelector('#editor'), {
                    extraPlugins: [MyCustomUploadAdapterPlugin],
                    toolbar: ['mediaEmbed', 'imageUpload', '|', 'bold', 'italic', 'link','insertTable', '|', 'heading', 'bulletedList', 'numberedList', 'blockQuote', 'fontFamily', 'undo', 'redo'],
                    table: tableConfig,
                    ckfinder: {
                        // Upload the images to the server using the CKFinder QuickUpload command.
                        uploadUrl: that.imgUploadUrl,

                        // Define the CKFinder configuration (if necessary).
                        options: {
                            //resourceType: 'Images',
                            chooseFiles: true,
                        },

                        openerMethod: 'popup'
                    },
                    mediaEmbed: {
                        previewsInData: true,
                        providers: [
                            {
                                name: 'allow-all',
                                // A URL regexp or an array of URL regexps:
                                url: /^.+/,
                                html: match => {
                                    const url = match['input'];
                                    console.log('media----' + url);
                                    return ('<video src="' + url + '" width="400" controls="controls"></video>');
                                }
                            },
                        ]
                    }
                })
                .then(editor => {
                    that.editor = editor;
                    console.log('editor---------' + editor);
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }
})




