package com.antonyjekov.nevix.player;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.view.View;

import com.antonyjekov.nevix.R;

/**
 * Created by Antony Jekov on 4/15/2014.
 */
public class InteractionIndicator extends View {

    Paint indicatorPaint;
    Paint ripplesPaint;
    boolean indicationScheduled;
    Rect currentButtonBounds;
    int radius;

    private final static int MAX_ALPHA = 180;
    private final static int ALPHA_STEP = 10;
    private int currentAlpha = MAX_ALPHA;

    public InteractionIndicator(Context context) {
        super(context);

        this.currentAlpha = MAX_ALPHA;
        this.indicatorPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        this.indicatorPaint.setColor(getResources().getColor(R.color.interaction_indicator));
        this.indicatorPaint.setAlpha(MAX_ALPHA);
        this.indicatorPaint.setStyle(Paint.Style.FILL);

        this.ripplesPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        this.ripplesPaint.setColor(Color.parseColor("#ffffff"));
        this.ripplesPaint.setStrokeWidth(getResources().getDimension(R.dimen.ripple_stroke_width));
        this.ripplesPaint.setStyle(Paint.Style.STROKE);
        this.ripplesPaint.setAlpha(MAX_ALPHA);
    }

    public void indicateButtonPress(Rect buttonBounds) {
        this.currentAlpha = MAX_ALPHA;
        this.indicationScheduled = true;
        this.currentButtonBounds = buttonBounds;
        this.radius = this.currentButtonBounds.height() >> 1;
        invalidate();
    }

    @Override
    protected void onDraw(Canvas canvas) {
        if (!indicationScheduled)
            return;

        if (this.currentAlpha > 0) {
            super.onDraw(canvas);
            this.currentAlpha -= ALPHA_STEP;
            this.indicatorPaint.setAlpha(this.currentAlpha);
            this.ripplesPaint.setAlpha(this.currentAlpha);

            int x = this.currentButtonBounds.centerX();
            int y = this.currentButtonBounds.centerY();

            canvas.drawCircle(x, y, radius, indicatorPaint);
            canvas.drawCircle(x, y, radius, ripplesPaint);

            this.radius += (int) (this.radius * .07);

            invalidate();
            return;
        }
    }
}
