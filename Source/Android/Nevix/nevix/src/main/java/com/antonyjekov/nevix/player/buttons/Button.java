package com.antonyjekov.nevix.player.buttons;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.Rect;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public abstract class Button {

    protected final Rect button;
    private Path shape;
    private Paint paint;
    protected int strokeWidth = 5;
    protected int padding = 20;

    public Button(Rect button) {
        this.button = button;
        shape = new Path();
        paint = new Paint();
        paint.setStrokeWidth(strokeWidth);
        paint.setStyle(Paint.Style.STROKE);
        paint.setColor(Color.RED);
    }

    public void renderSelf(Canvas canvas) {
        shape.reset();
        prepareShape(shape);
        canvas.drawPath(shape, paint);
    }

    protected abstract void prepareShape(Path shape);

    public abstract String command();

    public boolean isPointInButton(float x, float y) {
        return x >= button.left && x <= button.right &&
                y >= button.top && y <= button.bottom;
    }
}
