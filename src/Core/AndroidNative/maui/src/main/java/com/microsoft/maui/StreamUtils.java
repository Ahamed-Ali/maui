package com.microsoft.maui;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

/**
 * Utility class for handling .NET streams in Android with AndroidMarshalMethod.
 * Provides methods to detect and convert InputStreamAdapter objects to compatible streams.
 */
public class StreamUtils {
    
    /**
     * Checks if the InputStream is an InputStreamAdapter from mono.android.runtime.
     * This is used to detect streams that may cause issues in release builds
     * when AndroidMarshalMethod is enabled (default since .NET MAUI 10.0).
     * 
     * @param inputStream The stream to check
     * @return true if the stream is an InputStreamAdapter, false otherwise
     */
    public static boolean isInputStreamAdapter(InputStream inputStream) {
        if (inputStream == null) {
            return false;
        }
        String className = inputStream.getClass().getName();
        return "mono.android.runtime.InputStreamAdapter".equals(className);
    }
    
    /**
     * Converts an InputStream to a ByteArrayInputStream if it's an InputStreamAdapter.
     * If the stream is not an InputStreamAdapter, returns the original stream.
     * This ensures compatibility with Android image decoders and other native components.
     * 
     * @param inputStream The stream to process
     * @return A compatible InputStream (either the original or a converted ByteArrayInputStream)
     * @throws IOException If stream reading fails
     */
    public static InputStream ensureCompatibleStream(InputStream inputStream) throws IOException {
        if (!isInputStreamAdapter(inputStream)) {
            return inputStream;
        }
        
        return convertToByteArrayInputStream(inputStream);
    }
    
    /**
     * Converts an InputStream to a ByteArrayInputStream.
     * This ensures compatibility with Android components that may have issues
     * with InputStreamAdapter objects from .NET streams in release builds.
     * 
     * @param inputStream The stream to convert
     * @return A new ByteArrayInputStream containing the stream data
     * @throws IOException If stream reading fails or stream contains no data
     */
    public static InputStream convertToByteArrayInputStream(InputStream inputStream) throws IOException {
        if (inputStream == null) {
            throw new IOException("Input stream is null");
        }
        
        ByteArrayOutputStream buffer = new ByteArrayOutputStream();
        byte[] data = new byte[16384]; // 16KB buffer for good performance
        int nRead;
        
        try {
            while ((nRead = inputStream.read(data, 0, data.length)) != -1) {
                buffer.write(data, 0, nRead);
            }
            buffer.flush();
            
            byte[] streamData = buffer.toByteArray();
            if (streamData.length == 0) {
                throw new IOException("Stream contains no data");
            }
            
            return new ByteArrayInputStream(streamData);
        } finally {
            // Ensure the original stream is closed
            try {
                inputStream.close();
            } catch (IOException e) {
                // Log but don't rethrow - we want to return the converted stream
            }
        }
    }
}