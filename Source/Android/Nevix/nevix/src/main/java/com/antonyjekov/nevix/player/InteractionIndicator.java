package com.antonyjekov.nevix.player;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.Rect;
import android.view.View;

/**
 * Created by Antony Jekov on 4/15/2014.
 */
public class InteractionIndicator extends View {

    Paint indicatorPaint;
    Path indicatorPath;
    boolean maxAlphaReached;

    private final static int MAX_ALPHA = 40;
    private final static float ALPHA_STEP = .9f;
    private float currentAlpha;

    public InteractionIndicator(Context context) {
        super(context);

        this.currentAlpha = 0;
        this.indicatorPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        this.indicatorPaint.setColor(Color.WHITE);
        this.indicatorPaint.setAlpha(0);
        this.indicatorPaint.setStyle(Paint.Style.FILL);

        this.indicatorPath = new Path();
        this.maxAlphaReached = false;
    }

    public void indicateButtonPress(Rect buttonBounds) {
        this.indicatorPath.reset();
        this.currentAlpha = 0;
        this.indicatorPath.addCircle(buttonBounds.centerX(), buttonBounds.centerY(), buttonBounds.height() >> 1, Path.Direction.CW);

        invalidate();
    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);

        if (!this.maxAlphaReached){
            this.currentAlpha += ALPHA_STEP;
            this.indicatorPaint.setAlpha((int) this.currentAlpha);
            canvas.drawPath(this.indicatorPath, this.indicatorPaint);
            if (this.currentAlpha >= MAX_ALPHA)
                this.maxAlphaReached = true;

            invalidate();
        } else {
            this.currentAlpha -= ALPHA_STEP;
            this.indicatorPaint.setAlpha((int) this.currentAlpha);
            canvas.drawPath(this.indicatorPath, this.indicatorPaint);

            if (this.currentAlpha >= 0)
                invalidate();
        }
    }
}
