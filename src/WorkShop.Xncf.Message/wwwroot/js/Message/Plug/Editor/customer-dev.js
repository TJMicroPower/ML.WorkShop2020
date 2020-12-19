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
        };
    },
    mounted() {
        let that = this
        //that.initMyEditor()
        //that.initCKEditor()
        that.init()
    },
    methods: {
        init() {
            let that = this
            const config = {
                toolbar: {
                    items: ['bold', 'italic', '|', 'undo', 'redo'],
                    viewportTopOffset: 30,
                    shouldNotGroupWhenFull: true
                }
            }


            //配置参考：https://ckeditor.com/docs/ckeditor5/latest/api/module_core_editor_editorconfig-EditorConfig.html
            //上传匹配：https://ckeditor.com/docs/ckeditor5/latest/framework/guides/deep-dive/upload-adapter.html
            //https://ckeditor.com/docs/ckeditor5/latest/features/image-upload/ckfinder.html#configuring-the-image-upload-only
            //https://www.jianshu.com/p/47e25447b771
            ClassicEditor
                .create(document.querySelector('#editor'), {
                    extraPlugins: [MyCustomUploadAdapterPlugin],
                    toolbar: ['mediaEmbed', 'imageUpload', '|', 'bold', 'italic', 'link', '|', 'heading', 'bulletedList', 'numberedList', 'blockQuote', 'fontFamily', 'undo', 'redo'],
                    ckfinder: {
                        // Upload the images to the server using the CKFinder QuickUpload command.
                        uploadUrl: 'https://lelinggusu.milisx.xyz/api/v1/common/editorupload',
                        //uploadUrl: 'https://localhost:44311/api/v1/common/editorupload',

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

                                // To be defined only if the media are previewable:
                                //html: match => '<div style="position:relative; padding-bottom:100%; height:0">' +
                                //    '<iframe src="..." frameborder="0" ' +
                                //    'style="position:absolute; width:100%; height:100%; top:0; left:0">' +
                                //    '</iframe>' +
                                //    '</div>'

                                html: match => {
                                    const url = match['input'];
                                    console.log('media----' + url);
                                    return ('<video src="' + url + '" width="400" controls="controls"></video>');
                                }
                            },
                        ]
                        //
                    }
                })
                .then(editor => {

                    that.editor = editor;
                    //alert(that.editor.getData())
                    console.log('editor---------' + editor);
                })
                .catch(error => {
                    console.error(error);
                });
        },
        initCKEditor() {
            let that = this
            class UploadAdapter {
                constructor(loader) {
                    that.loader = loader
                }
                upload() {  //重置上传路径
                    return new Promise((resolve, reject, file) => {
                        debugger
                        var fileName = 'file_' + this.loader.file.lastModified
                        console.log(fileName);
                        //client().put(fileName, this.loader.file).then(result => {
                        //    resolve({
                        //        default: result.url
                        //    })
                        //}).catch(err => {
                        //    console.log(err)
                        //})

                        const data = new FormData();
                        data.append('upload', this.loader.file);
                        data.append('allowSize', 10);//允许图片上传的大小/兆
                        $.ajax({
                            url: 'https://localhost:44311/api/v1/common/editorupload',
                            type: 'POST',
                            data: data,
                            dataType: 'json',
                            processData: false,
                            contentType: false,
                            success: function (data) {
                                if (data.res) {
                                    resolve({
                                        default: data.url
                                    });
                                } else {
                                    reject(data.msg);
                                }

                            }
                        });
                    })
                }
                abort() {
                }
            }
            //初始化编辑器
            ClassicEditor
                .create(document.querySelector('#editor'), {
                    removePlugins: ['MediaEmbed'], //除去视频按钮
                    language: 'zh-cn',  // 中文
                })
                .then(editor => {
                    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
                        // Configure the URL to the upload script in your back-end here!
                        return new UploadAdapter(loader);
                    };
                    that.editor = editor;
                    //alert(that.editor.getData())
                    console.log('editor---------' + editor);
                })
                .catch(error => {
                    console.error(error);
                });
        },
        initMyEditor() {
            let that = this
            //初始化编辑器
            ClassicEditor
                .create(document.querySelector('#editor'), {
                    removePlugins: ['MediaEmbed'], //除去视频按钮
                    language: 'zh-cn',  // 中文
                    toolbar: ['ckfinder', 'mediaEmbed', 'imageUpload', '|', 'bold', 'italic', 'link', '|', 'heading', 'bulletedList', 'numberedList', 'blockQuote', 'fontFamily', 'undo', 'redo'],
                    ckfinder: {
                        // Upload the images to the server using the CKFinder QuickUpload command.
                        uploadUrl: 'https://lelinggusu.milisx.xyz/api/v1/common/editorupload',
                        //uploadUrl: 'https://localhost:44311/api/v1/common/EditorUpload',

                        // Define the CKFinder configuration (if necessary).
                        options: {
                            //resourceType: 'Images',
                            chooseFiles: true,
                        },

                        openerMethod: 'popup'
                    }
                })
                .then(editor => {
                    editor.execute('ckfinder');
                    console.log('hello editor')
                    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
                        // Configure the URL to the upload script in your back-end here!
                        return new MyUploadAdapter(loader);
                    };
                    that.editor = editor;
                    //alert(that.editor.getData())
                    console.log('editor---------' + editor);
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }
})




