package com.microsoft.maui.glide.stream;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.bumptech.glide.Priority;
import com.bumptech.glide.load.DataSource;
import com.bumptech.glide.load.Options;
import com.bumptech.glide.load.data.DataFetcher;
import com.bumptech.glide.load.model.ModelLoader;
import com.bumptech.glide.signature.ObjectKey;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class GlideInputStreamModelLoader implements ModelLoader<InputStream, InputStream> {
    @Nullable
    @Override
    public LoadData<InputStream> buildLoadData(@NonNull InputStream inputStream, int width, int height, @NonNull Options options) {
        return new LoadData<InputStream>(new ObjectKey(inputStream), new DataFetcher<InputStream>() {
            @Override
            public void loadData(@NonNull Priority priority, @NonNull DataCallback<? super InputStream> callback) {
                // Check if the inputStream is an InputStreamAdapter from mono.android.runtime
                // In release builds with AndroidMarshalMethod enabled, .NET streams are wrapped
                // in InputStreamAdapter objects that Glide cannot properly decode
                if (isInputStreamAdapter(inputStream)) {
                    try {
                        // Convert InputStreamAdapter to ByteArrayInputStream for reliable Glide decoding
                        InputStream processedStream = convertToByteArrayInputStream(inputStream);
                        callback.onDataReady(processedStream);
                    } catch (IOException e) {
                        callback.onLoadFailed(e);
                    }
                } else {
                    callback.onDataReady(inputStream);
                }
            }

            @Override
            public void cleanup() {
                try {
                    inputStream.close();
                } catch (IOException e) {
                }
            }

            @Override
            public void cancel() {

            }

            @NonNull
            @Override
            public Class<InputStream> getDataClass() {
                return InputStream.class;
            }

            @NonNull
            @Override
            public DataSource getDataSource() {
                return DataSource.LOCAL;
            }
        });
    }

    @Override
    public boolean handles(@NonNull InputStream inputStream) {
        return true;
    }

    /**
     * Checks if the InputStream is an InputStreamAdapter from mono.android.runtime.
     * This is used to detect streams that may cause issues with Glide in release builds.
     */
    private boolean isInputStreamAdapter(InputStream inputStream) {
        return inputStream.getClass().getName().equals("mono.android.runtime.InputStreamAdapter");
    }

    /**
     * Converts an InputStream to a ByteArrayInputStream.
     * This ensures that Glide gets a standard Java InputStream that it can properly decode.
     */
    private InputStream convertToByteArrayInputStream(InputStream inputStream) throws IOException {
        ByteArrayOutputStream buffer = new ByteArrayOutputStream();
        byte[] data = new byte[8192];
        int nRead;
        
        while ((nRead = inputStream.read(data, 0, data.length)) != -1) {
            buffer.write(data, 0, nRead);
        }
        
        buffer.flush();
        return new ByteArrayInputStream(buffer.toByteArray());
    }
}
