package com.microsoft.maui.glide.stream;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.bumptech.glide.Priority;
import com.bumptech.glide.load.DataSource;
import com.bumptech.glide.load.Options;
import com.bumptech.glide.load.data.DataFetcher;
import com.bumptech.glide.load.model.ModelLoader;
import com.bumptech.glide.signature.ObjectKey;

import java.io.IOException;
import java.io.InputStream;

import com.microsoft.maui.StreamUtils;

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
                try {
                    InputStream processedStream = StreamUtils.ensureCompatibleStream(inputStream);
                    callback.onDataReady(processedStream);
                } catch (IOException e) {
                    callback.onLoadFailed(e);
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
}
